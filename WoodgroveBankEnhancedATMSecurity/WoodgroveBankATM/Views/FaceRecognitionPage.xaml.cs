using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Streams;
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

namespace WoodgroveBankATM.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FaceRecognitionPage : Page
    {
        FaceAPIService faceClient = new FaceAPIService();
        AzureStorageService storageClient = new AzureStorageService();
        MessageDialog msg = new MessageDialog("");
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        //Define media capture object
        MediaCapture mediaCapture = new MediaCapture();
        public FaceRecognitionPage()
        {
            this.InitializeComponent();
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
                //Initialize media capture
                await mediaCapture.InitializeAsync();

                //Assign media capture to image control source
                PhotoPreview.Source = mediaCapture;

                //Start previewing
                await mediaCapture.StartPreviewAsync();

                base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //Dispose media capture when navigating away from this page
            mediaCapture.Dispose();

            base.OnNavigatedFrom(e);
        }
        private async void button_LogIn_Click(object sender, RoutedEventArgs e)
        {
            tb_LogInResult.Text = "";

            try
            {
                var memstream = new InMemoryRandomAccessStream();

                //Define image format (JPEG)
                ImageEncodingProperties imgFormat = ImageEncodingProperties.CreateJpeg();
                
                //Capture Photo to random access memory stream
                await mediaCapture.CapturePhotoToStreamAsync(imgFormat, memstream);

                IRandomAccessStream stream = memstream.CloneStream();
                if (stream != null)
                    if (!progressRing.IsActive)
                    {
                        progressRing.IsActive = true;

                        //Decode to bitmap format
                        BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                        SoftwareBitmap bitmap = await decoder.GetSoftwareBitmapAsync();
                        SoftwareBitmap bitmap2 = SoftwareBitmap.Convert(bitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                        SoftwareBitmapSource bmpSrc = new SoftwareBitmapSource();
                        await bmpSrc.SetBitmapAsync(bitmap2);
                        
                        //Set source for the image control to display the captured image
                        PhotoControl.Source = bmpSrc;

                        //Upload image to Azure blob and retrieve the image URL
                        string fileName = "temp" + DateTime.Now.ToString("ddMMyyyyhhmmtt") + ".png";
                        var imageUrl = await storageClient.uploadPhotoAsync("temp", fileName, stream.AsStream());

                        if (!(imageUrl is Exception))
                        {
                            //Detect face (if any) in the image
                            var FaceIDResponse = await faceClient.DetectFaceAsync(imageUrl.ToString());
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
                                    return;
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
                                    var candidate = result[0].candidates[0];
                                    string personID = localSettings.Values["PersonId"].ToString();

                                    //Check if the confidence level is greater than 0.5 and if the Person ID of 
                                    //the candidate does not match the person ID of the logged in user
                                    if ((candidate.confidence > 0.5) && (candidate.personId == personID))
                                        //Face matches - proceed to transaction
                                            Frame.Navigate(typeof(TransactionPage));
                                    else
                                     {
                                            //Face does not match
                                            msg.Title = "Unable to log in!";
                                            msg.Content = "Face does not match records or do not meet the required threshold!";
                                            await msg.ShowAsync();
                                            Frame.GoBack();
                                     }
                                    
                                }
                                else
                                {
                                    msg.Title = "Unable to log in!";
                                    msg.Content = responseContent;
                                    await msg.ShowAsync();
                                    Frame.GoBack();
                                }
                            }
                        }
                    }
            }
            catch (Exception ex)
            {
                msg.Title = "Unable to log in!";
                msg.Content = ex.Message;
                await msg.ShowAsync();
            }
            progressRing.IsActive = false;
            PhotoControl.Source = null;
        }
    }
}
