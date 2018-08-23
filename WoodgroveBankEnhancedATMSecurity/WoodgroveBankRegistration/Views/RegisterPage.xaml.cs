using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WoodgroveBankRegistration.ViewModels;
using WoodgrovePortable.Common;
using WoodgrovePortable.Models;
using WoodgrovePortable.Services;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WoodgroveBankRegistration.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegisterPage : Page
    {
        PersonGroupsVM pgvm = new PersonGroupsVM();
        FaceAPIService faceClient = new FaceAPIService();
        AzureStorageService storageService = new AzureStorageService();
        MessageDialog msg = new MessageDialog("");
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        public RegisterPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Handle back button
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                //Show UI in title bar if opted-in and in-app backstack is not empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
            }
            SystemNavigationManager.GetForCurrentView().BackRequested += RegisterPage_BackRequested;

            //Create the default person group if it doesn't exist
            await pgvm.InitializePersonGroupsAsync();

            //Load person groups in PersonGroupList Combo Box
            ComboBox_PersonGroups.ItemsSource = pgvm.PersonGroupList;
            foreach (var item in pgvm.PersonGroupList)
            {
                if (item.personGroupId == AppSettings.defaultPersonGroupID)
                    ComboBox_PersonGroups.SelectedItem = item;
            }

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

        private async void Button_RegisterUser_Click(object sender, RoutedEventArgs e)
        {
            //Disable register user button
            Button_RegisterUser.IsEnabled = false;

            //Encrypt the PIN
            string password = CryptographicBuffer.EncodeToHexString(HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha256).HashData(CryptographicBuffer.ConvertStringToBinary(PasswordBox_PIN.Password, BinaryStringEncoding.Utf8)));
            
            //Identify the selected person group
            var selectedGroup = (ComboBox_PersonGroups.SelectedItem as PersonGroupDetails).personGroupId;

            //Register the user profile - add the user entity to Azure table storage
            var result = await storageService.RegisterUserAsync(TextBox_Name.Text, password, selectedGroup);
            if (!(result is bool))
            {
                var ex = result as Exception;

                //Handle conflict if another user with the same name exists
                if (ex.Message.ToLower() == "conflict")
                {
                    msg.Title = "User already exists";
                    msg.Content = "Another user with the same username exists. Please try another username!";
                    await msg.ShowAsync();
                }
            }
            else
            {
                //Create the Person using Face API
                var response = await faceClient.CreatePersonAsync(selectedGroup, TextBox_Name.Text);
                var responseMessage = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    msg.Title = "User registered successfully!";
                    msg.Content = responseMessage;
                    await msg.ShowAsync();

                    //Log In after successfully registering
                    await LogInAsync();
                }
                else
                {
                    msg.Title = "Unable to register user!";
                    msg.Content = responseMessage;
                    await msg.ShowAsync();
                }
            }

            //Disable the button
            Button_RegisterUser.IsEnabled = true;
        }

        private async Task<bool> LogInAsync()
        {
            //Encrypt the PIN
            string password = CryptographicBuffer.EncodeToHexString(HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha256).HashData(CryptographicBuffer.ConvertStringToBinary(PasswordBox_PIN.Password, BinaryStringEncoding.Utf8)));

            //Invoke Log In method
            var result = await storageService.SignInAsync(TextBox_Name.Text, password);

            //If log in is successful
            if (result is bool)
            {
                var username = TextBox_Name.Text.ToLower().Replace(" ", "");

                //Get all persons in the person group
                var plist = await faceClient.ListPersonsAsync(AppSettings.defaultPersonGroupID);
                if (!(plist is bool))
                {
                    var personlist = plist as List<PersonDetails>;

                    //Iterate through the persons in the person group to check if username matches
                    foreach (var item in personlist)
                    {
                        if (item.userData == username)
                        {
                            //If a Person exists in Face API with the same username
                            //then save person ID and username in local settings
                            localSettings.Values["PersonId"] = item.personId;
                            localSettings.Values["UserName"] = item.userData;

                            //Navigate to Dashboard Page - Successful Log In
                            Frame.Navigate(typeof(Dashboard));
                        }
                    }
                }
            }
            else
            {
                //Display the failure message
                msg.Title = "Invalid Log In!";
                msg.Content = result.ToString();
                await msg.ShowAsync();
                return false;
            }
            return true;
        }
    }
}
