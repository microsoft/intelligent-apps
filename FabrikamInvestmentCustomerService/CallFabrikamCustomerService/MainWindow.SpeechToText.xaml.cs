//using Microsoft.CognitiveServices.SpeechRecognition;
using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CallFabrikamCustomerService
{
    public partial class MainWindow : Window
    {
        //These are fields needed for using speech recognition client library aka. Project Oxford
        //private SpeechRecognitionMode Mode;
        private string DefaultLocale;
        private string Language;
        //private MicrophoneRecognitionClient micClient;
        SoundPlayer thinking;
        private TaskCompletionSource<int> stopBaseRecognitionTaskCompletionSource;
        private SpeechRecognizer recognizer;

        /*
        private void StartMicrophone2()
        {
            //create and start mic recognition client
            if (micClient == null)
            {
                CreateMicrophoneRecoClient();
            }
            micClient.StartMicAndRecognition();
        }
        */

        private void StartMicrophone()
        {
            stopBaseRecognitionTaskCompletionSource = new TaskCompletionSource<int>();
            Task.Run(async () => { await CreateBaseReco().ConfigureAwait(false); });
        }

        /*
        private void StopMicrophone2()
        {
            //end mic recognition and dispose of client
            micClient.EndMicAndRecognition();
            micClient.Dispose();
        }
        */

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

        /// Creates Recognizer with baseline model and selected language:

        /// Creates a factory with subscription key and selected region

        /// If input source is audio file, creates recognizer with audio file otherwise with default mic

        /// Waits on RunRecognition

        /// </summary>

        private async Task CreateBaseReco()
        {
            thinking = new SoundPlayer(@"../../Resources/SpeechResponse_Thinking.wav");
            // Todo: suport users to specifiy a different region.

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
            //thinking.PlaySync();
            //this.EchoResponse(e);
            //recognizer.StopContinuousRecognitionAsync();
            this.WriteLine("Intermediate result: {0} ", e.Result.Text);

        }



        /// <summary>
        /// Logs the Final result
        /// </summary>

        private void FinalResultEventHandler(SpeechRecognitionResultEventArgs e)
        {
            thinking.PlaySync();
            this.EchoResponse(e);
            //recognizer.StartContinuousRecognitionAsync();
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

                //source.TrySetResult(0);

            }

        }



        private void SpeechDetectedEventHandler(RecognitionEventArgs e)

        {
            WriteLine(e.EventType == 0 ? "Mic Recording. Please start speaking." : "Mic Stopped.");
            //this.WriteLine(String.Format("Speech recognition: Speech event: {0}.", e.ToString()));
        }



        #endregion













        //Creates a new microphone client to start recoding and send send audio stream to Speech API automatically for text transcription
        private void CreateMicrophoneRecoClient()
        {
            thinking = new SoundPlayer(@"../../Resources/SpeechResponse_Thinking.wav");

            /*micClient = SpeechRecognitionServiceFactory.CreateMicrophoneClient(
                Mode,
                DefaultLocale,
                MicrosoftSpeechApiKey);

            // Event handlers for speech recognition results
            micClient.OnMicrophoneStatus += OnMicrophoneStatus;
            micClient.OnResponseReceived += OnMicShortPhraseResponseReceivedHandler;
            micClient.OnConversationError += OnConversationErrorHandler;
            */
        }

        /*//Event handler that is called when the microphone status is ready
        private void OnMicrophoneStatus(object sender, MicrophoneEventArgs e)
        {
            WriteLine(e.Recording ? "Mic Recording. Please start speaking." : "Mic Stopped.");
        }

        //This event handler gets called when full response audio is sent and transcribed
        private void OnMicShortPhraseResponseReceivedHandler(object sender, SpeechResponseEventArgs e)
        {
            thinking.PlaySync();

            this.EchoResponse(e);

            micClient.StartMicAndRecognition();
        }
       
        //writes the error from Speech API if any to the onscreen display
        private void OnConversationErrorHandler(object sender, SpeechErrorEventArgs e)
        {
            TransitionHangUpGui();

            this.WriteLine("--- Error received by OnConversationErrorHandler() ---");
            this.WriteLine("Error code: {0}", e.SpeechErrorCode.ToString());
            this.WriteLine("Error text: {0}", e.SpeechErrorText);
            this.WriteLine();
        }
        */
    }
}
