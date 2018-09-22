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
            thinking = new SoundPlayer(@"../../Resources/SpeechResponse_Thinking.wav");

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
        private void IntermediateResultEventHandler(SpeechRecognitionResultEventArgs e)
        {
            //TODO: end recognition
        }

        /// <summary>
        /// Logs the Final result
        /// </summary>
        private void FinalResultEventHandler(SpeechRecognitionResultEventArgs e)
        {
            thinking.PlaySync();

            //TODO: call EchoResponse to write out received response 

            //TODO: start mic recognition again
        }

        /// <summary>
        /// Logs Error events
        /// And sets the TaskCompletionSource to 0, in order to trigger Recognition Stop
        /// </summary>
        private void ErrorEventHandler(RecognitionErrorEventArgs e, TaskCompletionSource<int> source)
        {
            this.WriteLine("--- Error received by ErrorEventHandler() ---");
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
            //TODO: Writeline out mic status change to onscreen display
        }

        #endregion
    }
}
