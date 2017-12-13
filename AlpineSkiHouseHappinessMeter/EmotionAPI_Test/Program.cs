using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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

            Console.ReadKey();
        }

        /// <summary>
        /// Read the image from the passed Path and return it as a byte Array
        /// </summary>
        /// <param name="imageFilePath"></param>
        /// <returns></returns>
        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            
        }

        static async void CallEmotionAPI(string imageFilePath)
        {
            
        }
    }
}
