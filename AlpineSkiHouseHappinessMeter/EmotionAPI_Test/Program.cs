using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Common.Contract;

namespace EmotionAPI_Test
{
    public class Program
    {
        static string _apiKey;
        static string ApiKey
        {
            get
            {
                if (String.IsNullOrEmpty(_apiKey))
                    _apiKey = ConfigurationManager.AppSettings["apiKey"];
                return _apiKey;
            }
        }

        static void Main(string[] args)
        {
            // Get the image path
            Console.WriteLine("JPEG image file path:");
            string imageFilePath = Console.ReadLine();

            // Make the Emotion API call
            CallEmotionAPI(imageFilePath);

            // Make the Emotion API call by using Project Oxford Libraries
            CallProjectOxford(imageFilePath);

            Console.ReadKey();
        }

        /// <summary>
        /// Read the image from the passed Path and return it as a byte Array
        /// </summary>
        /// <param name="imageFilePath"></param>
        /// <returns></returns>
        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            using (FileStream stream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    return reader.ReadBytes((int)stream.Length);
                }
            }
        }

        static async void CallEmotionAPI(string imageFilePath)
        {
            // Declare a new HttpClient to communicate with the Emotion API
            HttpClient client = new HttpClient();

            // Add the API Key to the request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ApiKey);

            // This url will have to match the region where the cognitive services API was created
            string url = "https://westus.api.cognitive.microsoft.com/emotion/v1.0/recognize?";

            // Get the image as a byte array
            byte[] byteData = GetImageAsByteArray(imageFilePath);

            HttpResponseMessage JsonResponse;
            string responseContent;

            // Prepare the image and the request to query the Emotion API
            using (ByteArrayContent imageBytes = new ByteArrayContent(byteData))
            {
                // Add the proper media type
                imageBytes.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                // Call the emotion API by using the URL and the image content so it can analyze our image
                JsonResponse = await client.PostAsync(url, imageBytes);

                // Store the Emotion API result, this will be a JSON string that will contain the 
                // identified face (A rectangle surrounding the face) and a emotion score array
                responseContent = JsonResponse.Content.ReadAsStringAsync().Result;
            }

            // Show the raw JSON string
            Console.WriteLine();
            Console.WriteLine("JSON Response:");
            Console.WriteLine(responseContent);

            // Use the JSON result string and get the first and second array (face rectangle and emotion score)
            JToken root = JArray.Parse(responseContent).First;
            JToken faceDimension = root.First();
            JToken scores = root.Last();

            Console.WriteLine();
            Console.WriteLine("Face Rectangle");
            
            // Navigate through each face rectangle coordinate
            foreach (JToken size in faceDimension.First.Children())
            {
                Console.WriteLine(size);
            }

            Console.WriteLine();
            Console.WriteLine("Emotion Score");

            // Navigate through each emotion
            foreach (JToken score in scores.First.Children())
            {
                Console.WriteLine(score);
            }
        }

        static async void CallProjectOxford(string imageFilePath)
        {
            // Declare an EmotionServiceClient object, we will use this object to communicate with our Emotion API
            EmotionServiceClient client = new EmotionServiceClient(ApiKey);

            // Convert the Image file to a MemoryStream
            MemoryStream mem = new MemoryStream(GetImageAsByteArray(imageFilePath));
            // Store the result in an emotion list
            // With this 3 lines we already make all the communications with the API and store the result
            // ProjectOxford libraries help us to work easier with cognitive services APIs.
            IEnumerable<Emotion> emotionList = await client.RecognizeAsync(mem);

            // Print all the dimensions and emotion scores
            foreach (Emotion emotion in emotionList)
            {
                Console.WriteLine("Face");
                Console.WriteLine($"Top: {emotion.FaceRectangle.Top.ToString()}, Width: {emotion.FaceRectangle.Width.ToString()}" +
                    $"Left: {emotion.FaceRectangle.Left.ToString()}, Height: {emotion.FaceRectangle.Height.ToString()}");
                Console.WriteLine();

                Console.WriteLine("Emotion");
                Console.WriteLine($"Anger: {emotion.Scores.Anger.ToString()}," +
                    $"Contempt: {emotion.Scores.Contempt.ToString()}," +
                    $"Disgust: {emotion.Scores.Disgust.ToString()}," +
                    $"Fear: {emotion.Scores.Fear.ToString()}," +
                    $"Happiness: {emotion.Scores.Happiness.ToString()}," +
                    $"Neutral: {emotion.Scores.Neutral.ToString()}," +
                    $"Sadness: {emotion.Scores.Sadness.ToString()}," +
                    $"Surprise: {emotion.Scores.Surprise.ToString()}");
            }
        }
    }
}
