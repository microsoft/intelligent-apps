using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WoodgrovePortable.Models;
using WoodgrovePortable.Services;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WoodgroveBankRegistration.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AllUsers : Page
    {
        MessageDialog msg = new MessageDialog("");
        FaceAPIService faceClient = new FaceAPIService();
        AzureStorageService storageClient = new AzureStorageService();
        public AllUsers()
        {
            this.InitializeComponent();
        }
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            progressRing.IsActive = true;

            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame.CanGoBack)
            {
                // Show UI in title bar if opted-in and in-app backstack is not empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
            }
            SystemNavigationManager.GetForCurrentView().BackRequested += RegisterPage_BackRequested;

            //List all person groups
            var response = await faceClient.ListAllPersonGroupsAsync();
            if (response is HttpResponseMessage)
            {
                var result = response as HttpResponseMessage;
                if (result.IsSuccessStatusCode)
                {
                    var responseContent = await result.Content.ReadAsStringAsync();
                    var plist = JsonConvert.DeserializeObject<List<PersonGroupDetails>>(responseContent);
                    List_PersonGroups.ItemsSource = plist;
                }
            }
            progressRing.IsActive = false;

            base.OnNavigatedTo(e);
        }

        private void RegisterPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
            }
        }
        private async void List_PersonGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Reset person details
            ResetDetails();

            progressRing.IsActive = true;

            //Get selected item's Person Group ID
            var selectedItem = List_PersonGroups.SelectedItem as PersonGroupDetails;
            txt_GroupID.Text = selectedItem.personGroupId;

            //List all Persons in the selected Person Group
            var response = await faceClient.ListPersonsAsync(selectedItem.personGroupId);
            if (!(response is Exception))
            {
                if (!(response is bool))
                {
                    //Display list of Persons in List_Persons List View control
                    var personList = response as List<PersonDetails>;
                    List_Persons.ItemsSource = personList;
                }
            }

            progressRing.IsActive = false;
        }
        private void ResetDetails()
        {
            TextBlock_FaceCount.Text = "Face Count";
            TextBox_PersonName.Text = "";
            TextBox_PersonID.Text = "";
        }

        private void List_Persons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ResetDetails();
            var selectedPerson = List_Persons.SelectedItem as PersonDetails;
            if (selectedPerson != null)
            {
                TextBox_PersonName.Text = selectedPerson.name;
                TextBox_PersonID.Text = selectedPerson.personId;
                //Display count of Persisted Face IDs (Number of faces registered)
                TextBlock_FaceCount.Text = "Face Count " + selectedPerson.persistedFaceIds.Count().ToString();
            }
        }
        private async void Button_RemoveUser_Click(object sender, RoutedEventArgs e)
        {
            string personGroupID = txt_GroupID.Text;
            string personID = TextBox_PersonID.Text;
            string username = TextBox_PersonName.Text.ToLower().Replace(" ", "");

            if (List_Persons.SelectedItem != null)
            {
                //Load all faces registered for the user
                List<FaceEntity> faceList = new List<FaceEntity>();
                faceList = await storageClient.LoadFacesAsync(username, personID);

                foreach (var item in faceList)
                {
                    //Delete face images from blob storage
                    var success = await storageClient.DeleteImageAsync(personGroupID, item.ImageUrl);
                    if (success is Exception)
                    {
                        var ex = success as Exception;
                        msg.Title = "Unable to delete image from blob storage";
                        msg.Content = ex.Message.ToString();
                        await msg.ShowAsync();
                    }

                    //Delete Person Face from Face API
                    var deleteFaceFromFaceAPI = await faceClient.DeleteFaceAsync(personGroupID, personID, item.Id);
                    if (deleteFaceFromFaceAPI is Exception)
                    {
                        var ex = deleteFaceFromFaceAPI as Exception;
                        msg.Title = "Unable to delete person face using Face API";
                        msg.Content = ex.Message.ToString();
                        await msg.ShowAsync();
                    }
                    else
                    {
                        var response = deleteFaceFromFaceAPI as HttpResponseMessage;
                        var responseContent = await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode)
                        {
                            msg.Title = "Unable to delete person face using Face API";
                            msg.Content = responseContent;
                            await msg.ShowAsync();
                        }
                    }

                    //Delete face details from Azure storage
                    var deleteFaceStorage = await storageClient.DeleteFaceAsync(username, item.Id);
                    if (deleteFaceStorage is Exception)
                    {
                        var ex = deleteFaceStorage as Exception;
                        msg.Title = "Unable to delete face details from Azure table storage";
                        msg.Content = ex.Message.ToString();
                        await msg.ShowAsync();
                    }
                }

                //Delete Person from FaceAPI
                var faceResponse = await faceClient.DeletePersonAsync(personGroupID, personID);
                string faceResponseContent = await faceResponse.Content.ReadAsStringAsync();
                if (!faceResponse.IsSuccessStatusCode)
                {
                    msg.Title = "Unable to remove user from Face API";
                    msg.Content = faceResponseContent;
                    await msg.ShowAsync();
                }

                //Delete user from user table in Azure Table Storage
                var storageResponse = await storageClient.DeleteUserAsync(personGroupID, username);
                if (storageResponse is Exception)
                {
                    var ex = storageResponse as Exception;
                    msg.Title = "Unable to delete user from storage";
                    msg.Content = ex.Message.ToString();
                    await msg.ShowAsync();
                }

            }
        }
    }
}