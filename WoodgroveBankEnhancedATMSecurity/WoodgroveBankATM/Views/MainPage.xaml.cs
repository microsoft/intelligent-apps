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

namespace WoodgroveBankATM.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        AzureStorageService storageService = new AzureStorageService();
        FaceAPIService faceClient = new FaceAPIService();
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        MessageDialog msg = new MessageDialog("");
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Next_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //TODO: uncomment code in order to login with a pin

            /*if (!progressRing.IsActive)
            {
                progressRing.IsActive = true;

                //Disable controls
                TextBox_Name.IsEnabled = false;
                PasswordBox_PIN.IsEnabled = false;

                //Encrypt the PIN
                string password = CryptographicBuffer.EncodeToHexString(HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha256).HashData(CryptographicBuffer.ConvertStringToBinary(PasswordBox_PIN.Password, BinaryStringEncoding.Utf8)));

                //Invoke Log In method
                var result = await storageService.SignInAsync(TextBox_Name.Text, password);

                //If log in is successful verify Person has been created for the user using Face API
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

                                //Navigate to FaceRecognitionPage - Successful Log In
                                Frame.Navigate(typeof(FaceRecognitionPage));
                            }
                        }
                    }
                }
                else
                {
                    msg.Title = "Invalid Log In!";
                    msg.Content = result.ToString();
                    await msg.ShowAsync();
                }

                //Enable controls
                TextBox_Name.IsEnabled = true;
                PasswordBox_PIN.IsEnabled = true;

                progressRing.IsActive = false;
            }*/
        }
    }
}
