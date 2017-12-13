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
                      
            

            //TODO:  get list of all person groups
            //and set item source of List_PersonGroups to it



            progressRing.IsActive = false;

            base.OnNavigatedTo(e);
        }

        private void ResetDetails()
        {
            TextBlock_FaceCount.Text = "Face Count";
            TextBox_PersonName.Text = "";
            TextBox_PersonID.Text = "";
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
            //TODO: add in code to list all persons in a persongroup
        }

        private void List_Persons_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           //TODO: add in code to list all details of a person
        }
        private async void Button_RemoveUser_Click(object sender, RoutedEventArgs e)
        {
            string personGroupID = txt_GroupID.Text;
            string personID = TextBox_PersonID.Text;
            string username = TextBox_PersonName.Text.ToLower().Replace(" ", "");

            if (List_Persons.SelectedItem != null)
            {
                //TODO: delete user including face images and face details
                //from Azure blob storage, person face and person from Face API

            }
        }
    }
}