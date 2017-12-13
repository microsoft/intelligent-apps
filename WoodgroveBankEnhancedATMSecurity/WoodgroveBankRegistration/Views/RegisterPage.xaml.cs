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
                //This will be handled later
            }
            //Disable the button
            Button_RegisterUser.IsEnabled = true;
        }

    }
}
