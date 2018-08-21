// 
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.
// 
// Microsoft Cognitive Services: http://www.microsoft.com/cognitive
// 
// Microsoft Cognitive Services Github:
// https://github.com/Microsoft/Cognitive
// 
// Copyright (c) Microsoft Corporation
// All rights reserved.
// 
// MIT License:
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 

using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using ServiceHelpers.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ServiceHelpers
{
    public static class EmotionServiceHelper
    {
        #region Fill Properties

        #endregion

        public static int RetryCountOnQuotaLimitError = 6;
        public static int RetryDelayOnQuotaLimitError = 500;

        //Implement : You should declare a property, Task 4, Step 1
        private static FaceClient emotionClient { get; set; }
        

        static EmotionServiceHelper()
        {
            InitializeEmotionService();
        }

        public static Action Throttled;

        private const string baseUri = "https://<region>.api.cognitive.microsoft.com/face/v1.0";

        // Implement: PBI 2, Task 3, Step 2
        // Create an ApiKey property 
        private static string apiKey = "404aa004236c4dd685fb920004bc4463";
        public static string ApiKey
        {
            get { return apiKey; }
            set
            {
                var changed = apiKey != value;
                apiKey = value;
                if (changed)
                {
                    InitializeEmotionService();
                }
            }
        }

        // The list of Face attributes to return.
        private static IList<FaceAttributeType> faceAttributes =
            new FaceAttributeType[]
            {
                    //FaceAttributeType.Gender,
                    FaceAttributeType.Age,
                    FaceAttributeType.Smile,
                    FaceAttributeType.Emotion,
                    //FaceAttributeType.Glasses,
                    //FaceAttributeType.Hair
            };

        private static void InitializeEmotionService()
        {
            // Implement: PBI 2, Task 3, Step 3
            // Instatiate the EmotionServiceClient object and pass the API key to it so we can communicate with the emotion API.
            emotionClient = new FaceClient(
                                    new ApiKeyServiceClientCredentials(ApiKey),
                                    new System.Net.Http.DelegatingHandler[] { });

            emotionClient.BaseUri = new Uri(baseUri);
        }

        private static async Task<TResponse> RunTaskWithAutoRetryOnQuotaLimitExceededError<TResponse>(Func<Task<TResponse>> action)
        {
            // Optional Task, Implement: PBI 2, Task 3, Step 5
            int retriesLeft = 6;
            int delay = 500;

            TResponse response = default(TResponse);

            while (true)
            {
                try
                {
                    response = await action();
                    break;
                }
                catch (APIErrorException ex) when (ex.Response != null && ex.Response.StatusCode == (System.Net.HttpStatusCode)429 && retriesLeft > 0)
                {
                    ErrorTrackingHelper.TrackException(ex, "Emotion API throttling error");
                    if (retriesLeft == 1 && Throttled != null)
                    {
                        Throttled();
                    }

                    await Task.Delay(delay);
                    retriesLeft--;
                    delay *= 2;
                    continue;
                }
            }

            return response;
        }

        public static async Task<FaceEmotionData[]> RecognizeAsync(Func<Task<Stream>> imageStreamCallback)
        {
            // Implement: PBI 2, Task 3, Step 5
            // You should make a call to the EmotionServiceClient object that support a Stream as parameter to identify emotions
            //return await RunTaskWithAutoRetryOnQuotaLimitExceededError<EmotionData[]>(async () => await emotionClient.RecognizeAsync(await imageStreamCallback()));
            Stream imgData = await imageStreamCallback();
            IList<DetectedFace> result = await RunTaskWithAutoRetryOnQuotaLimitExceededError<IList<DetectedFace>>(async () => await emotionClient.Face.DetectWithStreamAsync(imgData, true, false, faceAttributes)); //.RecognizeAsync(await imageStreamCallback()));

            FaceEmotionData[] fed = new FaceEmotionData[result.Count];

            for (int i = 0; i < result.Count; i++)
            {

                FaceEmotionData emData = new FaceEmotionData
                {
                    Scores = new EmotionScores(),
                    FaceRectangle = new Rectangle()
                };

                emData.Scores.Anger = result[i].FaceAttributes.Emotion.Anger;
                emData.Scores.Contempt = result[i].FaceAttributes.Emotion.Contempt;
                emData.Scores.Disgust = result[i].FaceAttributes.Emotion.Disgust;
                emData.Scores.Fear = result[i].FaceAttributes.Emotion.Fear;
                emData.Scores.Happiness = result[i].FaceAttributes.Emotion.Happiness;
                emData.Scores.Neutral = result[i].FaceAttributes.Emotion.Neutral;
                emData.Scores.Sadness = result[i].FaceAttributes.Emotion.Sadness;
                emData.Scores.Surprise = result[i].FaceAttributes.Emotion.Surprise;

                emData.FaceRectangle.Height = result[i].FaceRectangle.Height;
                emData.FaceRectangle.Left = result[i].FaceRectangle.Left;
                emData.FaceRectangle.Top = result[i].FaceRectangle.Top;
                emData.FaceRectangle.Width = result[i].FaceRectangle.Width;

                fed[i] = emData;
            }

            return fed;
        }

        public static IEnumerable<EmotionData> ScoresToEmotionData(EmotionScores scores)
        {
            // Implement: PBI 2, Task 3, Step 4
            // Create a list of emotions and emotion scores to return as a results.
            // Use the JSON object result of the EmotionAPI_Test project to undestand how to build this list.
            List<EmotionData> result = new List<EmotionData>();
            result.Add(new EmotionData { EmotionName = "Anger", EmotionScore = scores.Anger });
            result.Add(new EmotionData { EmotionName = "Contempt", EmotionScore = scores.Contempt });
            result.Add(new EmotionData { EmotionName = "Disgust", EmotionScore = scores.Disgust });
            result.Add(new EmotionData { EmotionName = "Fear", EmotionScore = scores.Fear });
            result.Add(new EmotionData { EmotionName = "Happiness", EmotionScore = scores.Happiness });
            result.Add(new EmotionData { EmotionName = "Neutral", EmotionScore = scores.Neutral });
            result.Add(new EmotionData { EmotionName = "Sadness", EmotionScore = scores.Sadness });
            result.Add(new EmotionData { EmotionName = "Surprise", EmotionScore = scores.Surprise });

            return result;
        }
    }
}
