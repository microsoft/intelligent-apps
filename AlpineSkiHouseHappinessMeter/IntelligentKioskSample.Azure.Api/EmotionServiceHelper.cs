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

        // Implement: PBI 2, Task 3, Step 2
        // Create an ApiKey property 
        private static string apiKey = "404aa004236c4dd685fb920004bc4463";

        // Implement: PBI 2, Task 3, Step 9
        // Set correct region in Face API Uri
        private const string baseUri = "https://<region>.api.cognitive.microsoft.com/face/v1.0";

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

        static EmotionServiceHelper()
        {
            InitializeEmotionService();
        }

        public static Action Throttled;


        private static void InitializeEmotionService()
        {
            // Implement: PBI 2, Task 3, Step 3
            // Instatiate the EmotionServiceClient object and pass the API key to it so we can communicate with the emotion API.

            emotionClient.BaseUri = new Uri(baseUri);
        }

        private static async Task<TResponse> RunTaskWithAutoRetryOnQuotaLimitExceededError<TResponse>(Func<Task<TResponse>> action)
        {
            // Optional Task, Implement: PBI 2, Task 3, Step 5
            // Implement a retry logic to deal with transient events and too much request errors
        }

        public static async Task<FaceEmotionData[]> RecognizeAsync(Func<Task<Stream>> imageStreamCallback)
        {
            // Implement: PBI 2, Task 3, Step 5
            // You should make a call to the EmotionServiceClient object that support a Stream as parameter to identify emotions
        }

        public static IEnumerable<EmotionData> ScoresToEmotionData(EmotionScores scores)
        {
            // Implement: PBI 2, Task 3, Step 4
            // Create a list of emotions and emotion scores to return as a results.
            // Use the JSON object result of the EmotionAPI_Test project to undestand how to build this list.
        }
    }
}
