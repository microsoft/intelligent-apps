using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using WoodgrovePortable.Common;
using WoodgrovePortable.Models;
using WoodgrovePortable.Services;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WoodgroveBankRegistration.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Dashboard : Page
    {
        MessageDialog msg = new MessageDialog("");
        AzureStorageService storageClient = new AzureStorageService();
        FaceAPIService faceClient = new FaceAPIService();
        IRandomAccessStream stream;
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        public Dashboard()
        {
            this.InitializeComponent();
            DisplayLatestPhotos();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Enable back button
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame.CanGoBack)
            {
                // Show UI in title bar if opted-in and in-app backstack is not empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
            }
            SystemNavigationManager.GetForCurrentView().BackRequested += RegisterPage_BackRequested;

            base.OnNavigatedTo(e);
        }

        //Action to be taken when back button is clicked
        private void RegisterPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            while (rootFrame.BackStack.Count > 2)
            {
                rootFrame.BackStack.RemoveAt(rootFrame.BackStack.Count);
            }
            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
            }
        }

        private async void button_addPhoto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Initialize Camera Capture UI
                CameraCaptureUI cc = new CameraCaptureUI();

                //Set image format to png
                cc.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Png;

                //Capture photo
                var photo = await cc.CaptureFileAsync(CameraCaptureUIMode.Photo);

                //If no photo is captured, show error message
                if (photo == null)
                {
                    MessageDialog msg = new MessageDialog("No Photo", "No photo was captured!");
                    await msg.ShowAsync();
                    return;
                }
                else
                {
                    //Read photo as stream
                    stream = await photo.OpenAsync(FileAccessMode.Read);

                    //Decode to bitmap format
                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                    SoftwareBitmap bitmap = await decoder.GetSoftwareBitmapAsync();
                    SoftwareBitmap bitmap2 = SoftwareBitmap.Convert(bitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                    SoftwareBitmapSource bmpSrc = new SoftwareBitmapSource();
                    await bmpSrc.SetBitmapAsync(bitmap2);

                    //Set source for the image control to display the captured image
                    PhotoControl.Source = bmpSrc;
                }
            }
            catch (Exception ex)
            {
                msg.Title = "Error!";
                msg.Content = ex.Message;
                await msg.ShowAsync();
            }
        }

        private async void button_addface_Click(object sender, RoutedEventArgs e)
        {          
                //Disable all buttons while processing
                button_addface.IsEnabled = false;
                button_addPhoto.IsEnabled = false;
                
                //Upload function
                var storageUri = await storageClient.uploadPhotoAsync(AppSettings.defaultPersonGroupID, Guid.NewGuid().ToString()+".png", stream.AsStream());
                
                //If exception is returned, display error message
                if (storageUri is Exception)
                {
                    var ex = storageUri as Exception;
                    msg.Title = "Unable to upload photo!";
                    msg.Content = ex.Message;
                    await msg.ShowAsync();
                }
                else if (storageUri as string != "")
                {
                    //Add person face using Face API

                    //Upload to Azure Blob returns a Uri reference to the image
                    string Uri = storageUri.ToString();
                    
                    //Check if this face has already been registered with another user
                    bool faceExist = await CheckFaceExistsAsync(Uri);
                    
                    //If the face has not been registered with another account then add face to the person
                    if (!faceExist)
                    {
                        //Get Person ID of the logged in user from local settings
                        var personId = localSettings.Values["PersonId"].ToString();
                        
                        //Add Person Face to this Person
                        var result  = await faceClient.CreatePersonFaceAsync(AppSettings.defaultPersonGroupID, personId, Uri);
                        if (result.IsSuccessStatusCode)
                        {
                            var responseContent = await result.Content.ReadAsStringAsync();

                            //Deserialize response to PersistedFace object
                            var persistedFace = JsonConvert.DeserializeObject<PersistedFace>(responseContent);

                            if (persistedFace != null)
                            {
                                msg.Title = "Success!";
                                msg.Content = "Face registered successfully";

                                //Upload the registered image to Azure blob storage, with the persisted ID
                                await storageClient.uploadPhotoAsync(AppSettings.defaultPersonGroupID, persistedFace.persistedFaceId + ".png", stream.AsStream());
                                await msg.ShowAsync();

                                //Reset image control
                                PhotoControl.Source = null;
                                
                                //Save user face details to face table in Azure
                                var res = await storageClient.AddUserFaceAsync(localSettings.Values["UserName"].ToString(), localSettings.Values["PersonId"].ToString(), Uri, persistedFace.persistedFaceId);
                            }
                           
                        }
                        else
                        {
                            msg.Title = "Unable to register face!";
                            msg.Content = await result.Content.ReadAsStringAsync();
                            await msg.ShowAsync();
                        }
                    }
                }

            //Train Person Group after adding photo
            await faceClient.TrainPersonGroupAsync(AppSettings.defaultPersonGroupID);

            //Enable all buttons
            button_addface.IsEnabled = true;
            button_addPhoto.IsEnabled = true;

            //Display last three photos in image control
            DisplayLatestPhotos();
        }

        private async Task<bool> CheckFaceExistsAsync(string Uri)
        {
            //Detect face (if any) in the image
            var FaceIDResponse = await faceClient.DetectFaceAsync(Uri);
            if (FaceIDResponse.IsSuccessStatusCode)
            {
                //Detect face responds with face attributes if any face has been identified
                var results = JsonConvert.DeserializeObject<DetectFace.FaceClass[]>(await FaceIDResponse.Content.ReadAsStringAsync());

                //No face found in the image
                if (results.ToArray().Count() == 0)
                {
                    msg.Title = "Unable to register face!";
                    msg.Content = "No face found in the image, please try again!";
                    await msg.ShowAsync();
                    return true;
                }

                //Add all Face IDs to a list
                List<string> FaceIDList = new List<string>();
                foreach (var item in results)
                {
                    FaceIDList.Add(item.faceId);
                }

                //Check if the face matches any person in the person group with a confidence of 0.5F
                var candidateResponse = await faceClient.IdentifyFaceAsync(0.5F, FaceIDList.ToArray(), 2, AppSettings.defaultPersonGroupID);
                var responseContent = candidateResponse.Content.ReadAsStringAsync().Result;
                if (candidateResponse.IsSuccessStatusCode)
                {
                    //Identify function responds with a list of candidates to which the face matches and the confidence
                    var result = JsonConvert.DeserializeObject<IdentifyFaceResponseModel[]>(responseContent);

                    //Getting the candidate with highest confidence level
                    var candidates = result[0].candidates;
                    string personID = localSettings.Values["PersonId"].ToString();

                    //Check if the confidence level is greater than 0.5 and if the Person ID of 
                    //the candidate does not match the person ID of the logged in user
                    foreach (var candidate in candidates)
                        if ((candidate.confidence > 0.5) && (candidate.personId != personID))
                        {
                            msg.Title = "Unable to register face!";
                            msg.Content = "This face has been registered against another personID";
                            await msg.ShowAsync();
                            return true;
                        }
                }
                else
                {
                    msg.Title = "Unable to register face";
                    msg.Content = await candidateResponse.Content.ReadAsStringAsync();
                    await msg.ShowAsync();
                }
            }
            return false;
        }

        //Display last three photos
        private async void DisplayLatestPhotos()
        {
            List<FaceEntity> faceList = new List<FaceEntity>();
            faceList = await storageClient.LoadFacesAsync(localSettings.Values["UserName"].ToString(), localSettings.Values["PersonId"].ToString());
            if (faceList.Count > 0)
            {
                Image3.Source = new BitmapImage(new Uri(faceList.Last().ImageUrl));
                faceList.Remove(faceList.Last());
            }
            if(faceList.Count > 0)
            {
                Image2.Source = new BitmapImage(new Uri(faceList.Last().ImageUrl));
                faceList.Remove(faceList.Last());
            }
            if (faceList.Count > 0)
            {
                Image1.Source = new BitmapImage(new Uri(faceList.Last().ImageUrl));
                faceList.Remove(faceList.Last());
            }
        }
    }
}
