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
        
        public FaceRecognitionPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            //TODO: initialize and start capture of media stream

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            //TODO: Dispose media capture when navigating away from this page

            base.OnNavigatedFrom(e);
        }

        private async void button_LogIn_Click(object sender, RoutedEventArgs e)
        {
            tb_LogInResult.Text = "";

            try
            {
                //TODO: login using facial recognition
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
