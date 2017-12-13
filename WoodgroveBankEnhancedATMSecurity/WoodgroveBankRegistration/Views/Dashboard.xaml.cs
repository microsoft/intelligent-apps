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

            //DisplayLatestPhotos();
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
            //TODO: add in code to initialize the camera capture
            //and then display the image
        }

        private async void button_addface_Click(object sender, RoutedEventArgs e)
        {
            //Disable all buttons while processing
            button_addface.IsEnabled = false;
            button_addPhoto.IsEnabled = false;


            //TODO: Upload photo and add person face using Face API
            

            //Enable all buttons
            button_addface.IsEnabled = true;
            button_addPhoto.IsEnabled = true;

            //Display last three photos in image control
            //DisplayLatestPhotos();
        }

        private async void DisplayLatestPhotos()
        {
            List<FaceEntity> faceList = new List<FaceEntity>();


            //TODO: get all registered face details for the user


            //TODO: uncomment code
            //If faceList count is greater than 0, display the last image and remove the last item
            /*if (faceList.Count > 0)
            {
                Image3.Source = new BitmapImage(new Uri(faceList.Last().ImageUrl));
                faceList.Remove(faceList.Last());
            }

            //Repeat to display second last photo
            if (faceList.Count > 0)
            {
                Image2.Source = new BitmapImage(new Uri(faceList.Last().ImageUrl));
                faceList.Remove(faceList.Last());
            }

            //Repeat to display third last photo
            if (faceList.Count > 0)
            {
                Image1.Source = new BitmapImage(new Uri(faceList.Last().ImageUrl));
                faceList.Remove(faceList.Last());
            }*/
        }

        private async Task<bool> CheckFaceExistsAsync(string Uri)
        {
            //TODO: add code to check if face already matches a registered user


            return false;
        }
    }
}
