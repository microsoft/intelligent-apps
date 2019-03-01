using Microsoft.CognitiveServices.Speech;
using System;
using System.Diagnostics;
using System.Media;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace CallFabrikamCustomerService
{
    public partial class MainWindow : Window
    {
        //These are fields needed for using speech recognition client library
        private string DefaultLocale;
        private SpeechRecognizer recognizer;

        SoundPlayer thinking;
        

        private void StartSpeechRecognition()
        {
            string path = Assembly.GetExecutingAssembly().Location;
            string path1 = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path), "Resources\\SpeechResponse_Thinking.wav");
            thinking = new SoundPlayer(path1);

            try
            {
                //TODO: set the key, region, language and instantiate a recognizer


                //TODO: instantiate new instance of speech recognizer and 
                //keeping it for the lifetime until the main window closes


                //TODO: wire up event handlers to speech events


                //start speech recognition

            }
            catch (Exception ex)
            {
                this.WriteLine($"An exception occured:{ex}");
                Debug.WriteLine($"An exception occured:{ex}");
            }
        }

        private void StopSpeechRecognition()
        {
            //TODO: stop recognition


            //TODO: unsubscribe from events

        }

        #region Recognition Event Handlers

        private void RecognizingEventHandler(SpeechRecognitionEventArgs e)
        {
            //logs Intermediate Recognition results to Visual Studio Output Window
            Debug.WriteLine("\n    RecognizingEventHandler: {0}", e.Result);
        }

        private void RecognizedEventHandler(SpeechRecognitionEventArgs e)
        {
            //play the thinking sound to simulate processing transription
            thinking.PlaySync();

            //TODO: display the result in the main window

        }

        private void CanceledEventHandler(SpeechRecognitionCanceledEventArgs e)
        {
            //show errors in main window
            if (e.Reason == CancellationReason.Error)
            {
                this.WriteLine($"Recognition Canceled. Reason: {e.Reason}, ErrorDetails: {e.ErrorDetails}");
            }
        }

        private void SessionStartedEventHandler(SessionEventArgs e)
        {
            //writing out to the label control to show progress/status
            this.WriteLine("Session start detected.  Please start speaking.");
        }

        private void SessionStoppedEventHandler(SessionEventArgs e)
        {
            this.WriteLine("Session stop detected.");
        }

        private void SpeechStartDetectedEventHandler(RecognitionEventArgs e)
        {
            Debug.WriteLine("\n    Speech start detected.");
        }

        private void SpeechEndDetectedEventHandler(RecognitionEventArgs e)
        {
            Debug.WriteLine("\n    Speech end detected.");
        }

        #endregion
    }
}
