using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;

namespace CallFabrikamCustomerService
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string MicrosoftSpeechToTextEndpoint;
        private string MicrosoftSpeechApiKey;
        private string Region;

        private BitmapImage callButtonImage;
        private BitmapImage hangUpButtonImage;

        SoundPlayer dialTone;
        SoundPlayer ringing;

        public MainWindow()
        {
            InitializeComponent();

            //Initialize the speech end point & key from app.config
            MicrosoftSpeechToTextEndpoint = ConfigurationManager.AppSettings["MicrosoftSpeechToTextEndpoint"];
            MicrosoftSpeechApiKey = ConfigurationManager.AppSettings["MicrosoftSpeechApiKey"];
            Region = "westus";

            //Best practice to add event handler to dispose and cleanup resources whenever this window is closed
            this.Closing += OnMainWindowClosing;

            //Setup the green Call button image
            // Note BitmapImage.UriSource must be in a BeginInit/EndInit block.
            callButtonImage = new BitmapImage();
            callButtonImage.BeginInit();
            callButtonImage.UriSource = new Uri(@"/Resources/DialPad_Call.png", UriKind.RelativeOrAbsolute);
            callButtonImage.EndInit();

            //Setup the red hang up button image
            hangUpButtonImage = new BitmapImage();
            hangUpButtonImage.BeginInit();
            hangUpButtonImage.UriSource = new Uri(@"/Resources/DialPad_HangUp.png", UriKind.RelativeOrAbsolute);
            hangUpButtonImage.EndInit();

            //Setup dial & ringing tone
            string path = Assembly.GetExecutingAssembly().Location;
            string path1 = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path), "Resources\\Ringing_Phone-Mike_Koenig.wav");
            string path2 = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path), "Resources\\DialTone_18883226837.wav");
            dialTone = new SoundPlayer(path2);
            ringing = new SoundPlayer(path1);

            //Initialize speech to short phrase mode & default locale
            DefaultLocale = "en-US";
        }

        //Event handler that will cleanup speech client when window is closed; essentially closing app
        private void OnMainWindowClosing(object sender, CancelEventArgs e)
        {
            //cleanup resources
            dialTone.Dispose();
            ringing.Dispose();

            //check for null in case window closed without using recognizer
            if (this.thinking != null)
                thinking.Dispose();

            if (this.recognizer != null)
                recognizer.Dispose();

        }

        //Handle the call button click
        private async void keypadCallButton_Click(object sender, RoutedEventArgs e)
        {
            //dialing tone should run asap on another thread
            var dial = Task.Run(() => DialPhone());

            //animate dialing keypad
            await ShowKeypadDialing();

            //we should wait until the dialing tone has been completed before continue
            dial.Wait();

            StartSpeechRecognition();

            //transition calling to connected GUI
            TransitionCallGui();
        }

        //Handle the hang up button click
        private void keypadHangUpButton_Click(object sender, RoutedEventArgs e)
        {
            StopSpeechRecognition();

            //transition GUI back to ready to call
            TransitionHangUpGui();
        }

        private void TransitionCallGui()
        {
            // Switch background image from call to hangup 
            DialpadImage.Source = hangUpButtonImage;

            //hide call button by moving it's zindex to -ve and make hang up button & textbox on-screen print out visible
            Panel.SetZIndex(keypadCallButton, -1);
            Panel.SetZIndex(keypadHangUpButton, 1);
            Panel.SetZIndex(onScreenDisplay, 1);
        }

        private void TransitionHangUpGui()
        {
            //we should marshal any GUI updates back to the main thread with the Dispatcher
            Dispatcher.Invoke(() =>
            {
                // Switch background image back to call image 
                DialpadImage.Source = callButtonImage;

                //send hangup and on screen textbox output to the back to hide and set the call button to on top
                Panel.SetZIndex(keypadCallButton, 1);
                Panel.SetZIndex(keypadHangUpButton, -1);
                Panel.SetZIndex(onScreenDisplay, -1);

                onScreenDisplay.Text = "";
            });
        }


        //Play the dial tone in another thread
        private void DialPhone()
        {
            dialTone.PlaySync();
            ringing.PlaySync();
        }

        //This is a rudimentary way to animate touch and can be improved with WPF Animations
        private async Task ShowKeypadDialing()
        {
            //toll free number 1-888-FAB-NVES
            string customerServiceNumber = "1.8.8.8.3.2.2.6.8.3.7";
            string[] sequence = customerServiceNumber.Split('.');
            Ellipse key;
            for (int i = 0; i < sequence.Length; i++)
            {
                key = (Ellipse)this.FindName("keypad" + sequence[i]);
                key.Opacity = 0.5;
                await Task.Delay(200);
                key.Opacity = 0;
            }
        }

        //Writes the response result.
        private void EchoResponse(SpeechRecognitionEventArgs e)
        {
            WriteLine("Speech To Text Result:");
            //handle the case when there are no results. 
            //common situation is when there is a pause from user and audio captured has no speech in it
            if (e.Result.Text.Length == 0)
            {
                WriteLine("No phrase response is available.");
                WriteLine();
            }
            else
            {
                WriteLine(
                        "Text=\"{0}\"",
                        e.Result.Text);
            }
        }
        */
        //Writes the response result.
        private void EchoResponse(SpeechRecognitionResultEventArgs e)
        {
            WriteLine("Speech To Text Result:");
            //handle the case when there are no results. 
            //common situation is when there is a pause from user and audio captured has no speech in it
            if (e.Result.Text.Length == 0)
            {
                WriteLine("No phrase response is available.");
                WriteLine();
            }
            else
            {
                //speech to text usually returns an array of returns ranked highest first to lowest
                //we will print all of the results
                /*for (int i = 0; i < e.Result.Text.Length; i++)
                {
                    WriteLine(
                        "[{0}] Text=\"{1}\"",
                        i,
                        e.Result.Text);
                }*/
                //WriteLine();
                WriteLine(
                        "Text=\"{0}\"",
                        e.Result.Text);
            }
        }



        //Creates a line break
        internal void WriteLine()
        {
            WriteLine(string.Empty);
        }

        //Writes transcribed text to onscreen display
        internal void WriteLine(string format, params object[] args)
        {
            var formattedStr = string.Format(format, args);

            Dispatcher.Invoke(() =>
            {
                onScreenDisplay.Text += (formattedStr + "\n");
                onScreenDisplay.ScrollToEnd();
            });
        }

    }



}
