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

        }

        private async void button_addface_Click(object sender, RoutedEventArgs e)
        {          
              
        }

    }
}
