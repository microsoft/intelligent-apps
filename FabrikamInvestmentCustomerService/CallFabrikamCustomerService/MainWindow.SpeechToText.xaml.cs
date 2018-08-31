using Microsoft.CognitiveServices.SpeechRecognition;
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
        private SpeechRecognitionMode Mode;
        private string DefaultLocale;
        private MicrophoneRecognitionClient micClient;
        SoundPlayer thinking;

        private void StartMicrophone()
        {
            if (micClient == null)
            {
                CreateMicrophoneRecoClient();
            }
            micClient.StartMicAndRecognition();
        }

        private void StopMicrophone()
        {
            //end mic recognition and dispose of client
            micClient.EndMicAndRecognition();
            micClient.Dispose();
        }

        //Creates a new microphone client to start recoding and send send audio stream to Speech API automatically for text transcription
        private void CreateMicrophoneRecoClient()
        {
            thinking = new SoundPlayer(@"../../Resources/SpeechResponse_Thinking.wav");

            micClient = SpeechRecognitionServiceFactory.CreateMicrophoneClient(
                Mode,
                DefaultLocale,
                MicrosoftSpeechApiKey);

            // Event handlers for speech recognition results
            micClient.OnMicrophoneStatus += OnMicrophoneStatus;
            micClient.OnResponseReceived += OnMicShortPhraseResponseReceivedHandler;
            micClient.OnConversationError += OnConversationErrorHandler;
        }

        //Event handler that is called when the microphone status is ready
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
    }
}
