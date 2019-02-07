using Microsoft.CognitiveServices.Speech;
using System;
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
        SoundPlayer thinking;
        private TaskCompletionSource<int> stopBaseRecognitionTaskCompletionSource;
        private SpeechRecognizer recognizer;

        private void StartMicrophone()
        {
            //TODO: create and start mic recognition
            
        }

        private void StopMicrophone()
        {
            //TODO: end mic recognition and dispose of recognizer

        }

        //Creates a new microphone recognizer to start recoding and send send audio stream to Speech API automatically for text transcription
        private void CreateMicrophoneReco()
        {
            string path = Assembly.GetExecutingAssembly().Location;
            string path1 = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path), "Resources\\SpeechResponse_Thinking.wav");
            thinking = new SoundPlayer(path1);

            //TODO: use the speech recognition factory to create mic client


            //TODO: Call RunRecognizer Wire up Event handlers for speech recognition results

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

            //TODO: Wire up Event handlers for speech recognition results

            //start,wait,stop recognition
            await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);
            await source.Task.ConfigureAwait(false);
            await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);

            //TODO: Unsubscribe from Event handlers
        }

        #region Recognition Event Handlers

        /// <summary>
        /// Logs Intermediate Recognition results
        /// </summary>
        private void RecognizingEventHandler(SpeechRecognitionEventArgs e)
        {
            //TODO: end recognition
        }

        /// <summary>
        /// Logs the Final result
        /// </summary>
        private void RecognizedEventHandler(SpeechRecognitionEventArgs e)
        {
            thinking.PlaySync();

            //TODO: call EchoResponse to write out received response 

            //TODO: start mic recognition again
        }

        /// <summary>
        /// Logs Cancel events
        /// And sets the TaskCompletionSource to 0, in order to trigger Recognition Stop
        /// </summary>
        private void CanceledEventHandler(SpeechRecognitionCanceledEventArgs e, TaskCompletionSource<int> source)
        {
            this.WriteLine($"\n    Recognition Canceled. Reason: {e.Reason.ToString()}, CanceledReason: {e.Reason}");
            source.TrySetResult(0);
            TransitionHangUpGui();
        }

        private void SessionStartedEventHandler(SessionEventArgs e, TaskCompletionSource<int> source)
        {
            Console.WriteLine("\n    Session started event.");
        }

        private void SessionStoppedEventHandler(SessionEventArgs e, TaskCompletionSource<int> source)
        {
            Console.WriteLine("\n    Session stopped event.");
        }

        private void SpeechStartDetectedEventHandler(RecognitionEventArgs e)
        {
            Console.WriteLine("\n    Speech start detected.");
        }

        private void SpeechEndDetectedEventHandler(RecognitionEventArgs e)
        {
            Console.WriteLine("\n    Speech end detected.");
        }

        #endregion
    }
}
