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
            stopBaseRecognitionTaskCompletionSource = new TaskCompletionSource<int>();
            Task.Run(async () => { await CreateMicrophoneReco().ConfigureAwait(false); });
        }

        private void StopMicrophone()
        {
            //end mic recognition
            recognizer.StopContinuousRecognitionAsync().Wait();

            // unsubscribe from events
            recognizer.Recognizing -= (sender, e) => RecognizingEventHandler(e);
            recognizer.Recognized -= (sender, e) => RecognizedEventHandler(e);
            recognizer.Canceled -= (sender, e) => CanceledEventHandler(e, stopBaseRecognitionTaskCompletionSource);
            recognizer.SessionStarted -= (sender, e) => SessionStartedEventHandler(e, stopBaseRecognitionTaskCompletionSource);
            recognizer.SessionStopped -= (sender, e) => SessionStoppedEventHandler(e, stopBaseRecognitionTaskCompletionSource);
            recognizer.SpeechStartDetected -= (sender, e) => SpeechStartDetectedEventHandler(e);
            recognizer.SpeechEndDetected -= (sender, e) => SpeechEndDetectedEventHandler(e);

            stopBaseRecognitionTaskCompletionSource.TrySetResult(0);
        }

        /// <summary>
        /// Creates Recognizer with English language and microphone
        /// Creates a config with subscription key and selected region
        /// Waits on RunRecognition
        /// </summary>
        private async Task CreateMicrophoneReco()
        {
            string path = Assembly.GetExecutingAssembly().Location;
            string path1 = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(path), "Resources\\SpeechResponse_Thinking.wav");
            thinking = new SoundPlayer(path1);
            // Todo: suport users to specifiy a different region.
            try
            {
                var speechConfig = SpeechConfig.FromSubscription(this.MicrosoftSpeechApiKey, this.Region);
                speechConfig.SpeechRecognitionLanguage = this.DefaultLocale;

                SpeechRecognizer basicRecognizer;

                using (basicRecognizer = new SpeechRecognizer(speechConfig))
                {
                    await this.RunRecognizer(basicRecognizer, stopBaseRecognitionTaskCompletionSource).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                WriteLine($"An exception occured:{ex}");
                Console.WriteLine($"An exception occured:{ex}");
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
            recognizer.Recognizing += (sender, e) => RecognizingEventHandler(e);
            recognizer.Recognized += (sender, e) => RecognizedEventHandler(e);
            recognizer.Canceled += (sender, e) => CanceledEventHandler(e, source);
            recognizer.SessionStarted += (sender, e) => SessionStartedEventHandler(e, source);
            recognizer.SessionStopped += (sender, e) => SessionStoppedEventHandler(e, source);
            recognizer.SpeechStartDetected += (sender, e) => SpeechStartDetectedEventHandler(e);
            recognizer.SpeechEndDetected += (sender, e) => SpeechEndDetectedEventHandler(e);

            //start,wait,stop recognition
            await recognizer.StartContinuousRecognitionAsync().ConfigureAwait(false);
            await source.Task.ConfigureAwait(false);
            await recognizer.StopContinuousRecognitionAsync().ConfigureAwait(false);


            // unsubscribe from events
            recognizer.Recognizing -= (sender, e) => RecognizingEventHandler(e);
            recognizer.Recognized -= (sender, e) => RecognizedEventHandler(e);
            recognizer.Canceled -= (sender, e) => CanceledEventHandler(e, source);
            recognizer.SessionStarted -= (sender, e) => SessionStartedEventHandler(e, source);
            recognizer.SessionStopped -= (sender, e) => SessionStoppedEventHandler(e, source);
            recognizer.SpeechStartDetected -= (sender, e) => SpeechStartDetectedEventHandler(e);
            recognizer.SpeechEndDetected -= (sender, e) => SpeechEndDetectedEventHandler(e);
        }

        #region Recognition Event Handlers

        /// <summary>
        /// Logs Intermediate Recognition results
        /// </summary>
        private void RecognizingEventHandler(SpeechRecognitionEventArgs e)
        {
            recognizer.StopContinuousRecognitionAsync().Wait();
        }

        /// <summary>
        /// Logs the Final result
        /// </summary>
        private void RecognizedEventHandler(SpeechRecognitionEventArgs e)
        {
            thinking.PlaySync();
            this.EchoResponse(e);
        }

        /// <summary>
        /// Logs Error events
        /// And sets the TaskCompletionSource to 0, in order to trigger Recognition Stop
        /// </summary>
        private void CanceledEventHandler(SpeechRecognitionCanceledEventArgs e, TaskCompletionSource<int> source)
        {
            WriteLine($"\n    Recognition Canceled. Reason: {e.Reason.ToString()}, CanceledReason: {e.Reason}");
            source.TrySetResult(0);
            TransitionHangUpGui();
        }

        private void SessionStartedEventHandler(SessionEventArgs e, TaskCompletionSource<int> source)
        {
            WriteLine("Session start detected.  Please start speaking.");
        }

        private void SessionStoppedEventHandler(SessionEventArgs e, TaskCompletionSource<int> source)
        {
            WriteLine("Session stop detected.");
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
