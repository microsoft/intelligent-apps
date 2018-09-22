using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace CallFabrikamCustomerService
{
    public partial class MainWindow : Window
    {
        //fields needed to do REST call to Speech API
        private HttpClient httpClient;
        private HttpClientHandler httpHandler;
        private SoundPlayer speech;


        private string accessToken;
        private Timer accessTokenRenewer;

        //Access token expires every 10 minutes. Renew it every 9 minutes only.
        private const int RefreshTokenDuration = 9;

        private void CreateSpeechClient()
        {
            //TODO: initialize cookie container, http client handler, http client and get an access token


            //TODO: add auto-renewal of access token needed to connect to text to speech API
            //This auto-renew the Speech API access token needed when doing a POST
            //The access token only last for 10min so we setup a timer to renew the it every 9min

        }

        public Task PlaySpeechAudio(string Text)
        {
            if (httpClient == null)
                CreateSpeechClient();

            //TODO: cleanup the headers since we are reusing the HttpClient


            //TODO: add http headers specific for text to speech api
            //these are the minimum number of Speech API headers to include


            //TODO: initialize a new instance of http request message
            HttpRequestMessage request;


            //TODO: send the request, read the response stream and pass it to sound player to play the audio to speaker
            Task<HttpResponseMessage> httpTask = null;
            Task<Task> saveTask = null;


            return saveTask;
        }

        //Helps generate SSML for posting to Text-to-Speech API
        private string GenerateSsml(string locale, string gender, string name, string text)
        {
            XDocument ssmlDoc = new XDocument();

            //TODO: create SSML XML document that will be the payload for posting to speech api


            return ssmlDoc.ToString();
        }

        //Callback method when the timer fires every 9min to renew speech token
        private void OnTokenExpiredCallback(object stateInfo)
        {
            //TODO: do http post to get new token and assign new token to accessToken
            
        }

        //Helper method to get extract speech access token
        private string HttpPost(string accessUri, string apiKey)
        {
            // Prepare OAuth request
            WebRequest webRequest = WebRequest.Create(accessUri);
            webRequest.Method = "POST";
            webRequest.ContentLength = 0;
            webRequest.Headers["Ocp-Apim-Subscription-Key"] = apiKey;

            using (WebResponse webResponse = webRequest.GetResponse())
            {
                using (Stream stream = webResponse.GetResponseStream())
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        byte[] waveBytes = null;
                        int count = 0;
                        do
                        {
                            byte[] buf = new byte[1024];
                            count = stream.Read(buf, 0, 1024);
                            ms.Write(buf, 0, count);
                        } while (stream.CanRead && count > 0);

                        waveBytes = ms.ToArray();

                        return Encoding.UTF8.GetString(waveBytes);
                    }
                }
            }
        }



    }
}
