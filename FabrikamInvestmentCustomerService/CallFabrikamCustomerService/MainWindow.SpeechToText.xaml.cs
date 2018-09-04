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
                //set the key, region, language and instantiate a recognizer
                var speechConfig = SpeechConfig.FromSubscription(this.MicrosoftSpeechApiKey, this.Region);
                speechConfig.SpeechRecognitionLanguage = this.DefaultLocale;

                //instantiate new instance of speech recognizer and 
                //keeping it for the lifetime until the main window closes
                recognizer = new SpeechRecognizer(speechConfig);

                //wire up event handlers to speech events
                recognizer.Recognizing += (sender, e) => RecognizingEventHandler(e);
                recognizer.Recognized += (sender, e) => RecognizedEventHandler(e);
                recognizer.Canceled += (sender, e) => CanceledEventHandler(e);
                recognizer.SessionStarted += (sender, e) => SessionStartedEventHandler(e);
                recognizer.SessionStopped += (sender, e) => SessionStoppedEventHandler(e);
                recognizer.SpeechStartDetected += (sender, e) => SpeechStartDetectedEventHandler(e);
                recognizer.SpeechEndDetected += (sender, e) => SpeechEndDetectedEventHandler(e);

                //start speech recognition
                recognizer.StartContinuousRecognitionAsync();
            }
            catch (Exception ex)
            {
                this.WriteLine($"An exception occured:{ex}");
                Debug.WriteLine($"An exception occured:{ex}");
            }
        }
        */

        private void StopSpeechRecognition()
        {
            //stop recognition
            recognizer.StopContinuousRecognitionAsync().Wait();

            //unsubscribe from events
            recognizer.Recognizing -= (sender, e) => RecognizingEventHandler(e);
            recognizer.Recognized -= (sender, e) => RecognizedEventHandler(e);
            recognizer.Canceled -= (sender, e) => CanceledEventHandler(e);
            recognizer.SessionStarted -= (sender, e) => SessionStartedEventHandler(e);
            recognizer.SessionStopped -= (sender, e) => SessionStoppedEventHandler(e);
            recognizer.SpeechStartDetected -= (sender, e) => SpeechStartDetectedEventHandler(e);
            recognizer.SpeechEndDetected -= (sender, e) => SpeechEndDetectedEventHandler(e);
        }

        private void StopMicrophone()
        {
            recognizer.StopContinuousRecognitionAsync().Wait();

            // unsubscribe from events
            recognizer.IntermediateResultReceived -= (sender, e) => IntermediateResultEventHandler(e);
            recognizer.FinalResultReceived -= (sender, e) => FinalResultEventHandler(e);
            recognizer.RecognitionErrorRaised -= (sender, e) => ErrorEventHandler(e, stopBaseRecognitionTaskCompletionSource);
            recognizer.OnSessionEvent -= (sender, e) => SessionEventHandler(e, stopBaseRecognitionTaskCompletionSource);
            recognizer.OnSpeechDetectedEvent -= (sender, e) => SpeechDetectedEventHandler(e);

            stopBaseRecognitionTaskCompletionSource.TrySetResult(0);
        }

        /// <summary>
        /// Creates Recognizer with English language and microphone
        /// Creates a factory with subscription key and selected region
        /// Waits on RunRecognition
        /// </summary>
        private async Task CreateMicrophoneReco()
        {
            thinking = new SoundPlayer(@"../../Resources/SpeechResponse_Thinking.wav");
            // Todo: support users to specifiy a different region.

            var basicFactory = SpeechFactory.FromSubscription(this.MicrosoftSpeechApiKey, this.Region);

            SpeechRecognizer basicRecognizer;

            using (basicRecognizer = basicFactory.CreateSpeechRecognizer(this.DefaultLocale))
            {

                await this.RunRecognizer(basicRecognizer, stopBaseRecognitionTaskCompletionSource).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Subscribes to Recognition Events
        /// Starts the Recognition and waits until Final Result is received, then Stops recognition
        /// </summary>
        /// <param name="recognizer">Recognizer object</param>
        /// <param name="recoType">Type of Recognizer</param>
        ///  <value>
        ///   <c>Base</c> if Baseline model; otherwise, <c>Custom</c>.
        /// </value>
        private async Task RunRecognizer(SpeechRecognizer recogniz, TaskCompletionSource<int> source)
        {
            recognizer = recogniz;

            //subscribe to events
            recognizer.IntermediateResultReceived += (sender, e) => IntermediateResultEventHandler(e);
            recognizer.FinalResultReceived += (sender, e) => FinalResultEventHandler(e);
            recognizer.RecognitionErrorRaised += (sender, e) => ErrorEventHandler(e, source);
            recognizer.OnSessionEvent += (sender, e) => SessionEventHandler(e, source);
            recognizer.OnSpeechDetectedEvent += (sender, e) => SpeechDetectedEventHandler(e);
            
            
            //start,wait,stop recognition
            await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);
            await source.Task.ConfigureAwait(false);
            await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);


            // unsubscribe from events
            recognizer.IntermediateResultReceived -= (sender, e) => IntermediateResultEventHandler(e);
            recognizer.FinalResultReceived -= (sender, e) => FinalResultEventHandler(e);
            recognizer.RecognitionErrorRaised -= (sender, e) => ErrorEventHandler(e, source);
            recognizer.OnSessionEvent -= (sender, e) => SessionEventHandler(e, source);
            recognizer.OnSpeechDetectedEvent -= (sender, e) => SpeechDetectedEventHandler(e);
        }

        #region Recognition Event Handlers

        /// <summary>
        /// Logs Intermediate Recognition results
        /// </summary>
        private void IntermediateResultEventHandler(SpeechRecognitionResultEventArgs e)
        {
            recognizer.StopContinuousRecognitionAsync();
            //this.WriteLine("Intermediate result: {0} ", e.Result.Text);
        }

        /// <summary>
        /// Logs the Final result
        /// </summary>
        private void FinalResultEventHandler(SpeechRecognitionResultEventArgs e)
        {
            thinking.PlaySync();
            this.EchoResponse(e);
        }

        /// <summary>
        /// Logs Error events
        /// And sets the TaskCompletionSource to 0, in order to trigger Recognition Stop
        /// </summary>
        private void ErrorEventHandler(RecognitionErrorEventArgs e, TaskCompletionSource<int> source)
        {
            source.TrySetResult(0);
            TransitionHangUpGui();
        }

        /// <summary>
        /// If SessionStoppedEvent is received, sets the TaskCompletionSource to 0, in order to trigger Recognition Stop
        /// </summary>
        private void SessionEventHandler(SessionEventArgs e, TaskCompletionSource<int> source)
        {
            if (e.EventType == SessionEventType.SessionStoppedEvent)
            {
                source.TrySetResult(0);
            }
        }

        private void SpeechDetectedEventHandler(RecognitionEventArgs e)
        {
            WriteLine(e.EventType == 0 ? "Mic Recording. Please start speaking." : "Mic Stopped.");
        }

        #endregion













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

            //display the result in the main window
            this.EchoResponse(e);
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
