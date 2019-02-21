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


        private string apiKey;
        private string accessToken;
        private Timer accessTokenRenewer;

        //Access token expires every 10 minutes. Renew it every 9 minutes only.
        private const int RefreshTokenDuration = 9;

        private void CreateSpeechClient()
        {
            var cookieContainer = new CookieContainer();
            httpHandler = new HttpClientHandler() { CookieContainer = new CookieContainer(), UseProxy = false };
            httpClient = new HttpClient(httpHandler);

            apiKey = MicrosoftSpeechApiKey;
            accessToken = HttpPost(MicrosoftSpeechAccessTokenEndpoint, apiKey);

            //This auto-renew the Speech API access token needed when doing a POST
            //The access token only last for 10min so we setup a timer to renew the it every 9min
            accessTokenRenewer = new Timer(new TimerCallback(OnTokenExpiredCallback),
                                           this,
                                           TimeSpan.FromMinutes(RefreshTokenDuration),
                                           TimeSpan.FromMilliseconds(-1));
        }

        public Task PlaySpeechAudioAsync(string Text)
        {
            if (httpClient == null)
                CreateSpeechClient();

            httpClient.DefaultRequestHeaders.Clear();

            //these are the minimum number of Speech API headers to include
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/ssml+xml");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("X-Microsoft-OutputFormat", "riff-16khz-16bit-mono-pcm");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "IntelligentApps/FabrikamInvestmentCustomerService");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + accessToken);
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Host", "westus.tts.speech.microsoft.com");


            var request = new HttpRequestMessage(HttpMethod.Post, MicrosoftTextToSpeechEndpoint)
            {
                //we are making a few default assumptions here such as using English, Femail & the speech voice to use
                //for additional choices refer https://docs.microsoft.com/en-us/azure/cognitive-services/speech-service/supported-languages#text-to-speech
                Content = new StringContent(GenerateSsml("en-US", "Female", "Microsoft Server Speech Text to Speech Voice (en-US, ZiraRUS)", Text))
            };

            var httpTask = httpClient.SendAsync(request);

            var saveTask = httpTask.ContinueWith(
                async (responseMessage, token) =>
                {
                    try
                    {
                        if (responseMessage.IsCompleted && responseMessage.Result != null && responseMessage.Result.IsSuccessStatusCode)
                        {
                            var httpStream = await responseMessage.Result.Content.ReadAsStreamAsync().ConfigureAwait(false);
                            speech = new SoundPlayer(httpStream);
                            speech.PlaySync();
                        }
                        else
                        {
                            this.WriteLine("Service returned {0}", responseMessage.Result.StatusCode);
                        }
                    }
                    catch (Exception ex)
                    {
                        this.WriteLine(ex.GetBaseException().Message);
                    }
                    finally
                    {
                        responseMessage.Dispose();
                        request.Dispose();
                    }
                },
                TaskContinuationOptions.AttachedToParent,
                CancellationToken.None);

            return saveTask;
        }

        //Helps generate SSML for posting to Text-to-Speech API
        private string GenerateSsml(string locale, string gender, string name, string text)
        {
            var ssmlDoc = new XDocument(
                              new XElement("speak",
                                  new XAttribute("version", "1.0"),
                                  new XAttribute(XNamespace.Xml + "lang", "en-US"),
                                  new XElement("voice",
                                      new XAttribute(XNamespace.Xml + "lang", locale),
                                      new XAttribute(XNamespace.Xml + "gender", gender),
                                      new XAttribute("name", name),
                                      text)));
            return ssmlDoc.ToString();
        }

        //Callback method when the timer fires every 9min to renew speech token
        private void OnTokenExpiredCallback(object stateInfo)
        {
            try
            {
                string newAccessToken = HttpPost(MicrosoftSpeechAccessTokenEndpoint, apiKey);
                //swap the new token with old one
                //Note: the swap is thread unsafe
                accessToken = newAccessToken;
            }
            catch (Exception ex)
            {
                this.WriteLine(string.Format("Failed renewing access token. Details: {0}", ex.Message));
            }
            finally
            {
                try
                {
                    accessTokenRenewer.Change(TimeSpan.FromMinutes(RefreshTokenDuration), TimeSpan.FromMilliseconds(-1));
                }
                catch (Exception ex)
                {
                    this.WriteLine(string.Format("Failed to reschedule the timer to renew access token. Details: {0}", ex.Message));
                }
            }
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
