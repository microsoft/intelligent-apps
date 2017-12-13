using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using WoodgrovePortable.Common;
using WoodgrovePortable.Models;
using WoodgrovePortable.Services;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WoodgroveBankRegistration.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LogInPage : Page
    {
        AzureStorageService storageService = new AzureStorageService();
        FaceAPIService faceClient = new FaceAPIService();
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        MessageDialog msg = new MessageDialog("");
        public LogInPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
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

            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
            }
        }

        private async void Button_LogIn_Click(object sender, RoutedEventArgs e)
        {
            //Disable log in button
            Button_LogIn.IsEnabled = false;

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
            }

            //Enable the log in button
            Button_LogIn.IsEnabled = true;
        }

    }
}
