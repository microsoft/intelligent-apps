using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceHelpers.Tests
{
    [TestClass]
    public class FaceApiTests
    {
        private const string baseUri =
            "https://westcentralus.api.cognitive.microsoft.com/face/v1.0";

        private readonly IFaceClient faceClient = new FaceClient(
            new ApiKeyServiceClientCredentials("404aa004236c4dd685fb928884bc4463"),
            new System.Net.Http.DelegatingHandler[] { });
        private readonly string ImageFilePath = @"C:\Dev\intelligent-apps\ServiceHelpers.Tests\Assets\demo_face_api.png";
        IList<DetectedFace> faceList = null;   // The list of detected faces.
        String[] faceDescriptions;      // The list of descriptions for the detected faces.
        double resizeFactor;            // The resize factor for the displayed image.

        [TestMethod]
        public async Task TestMethod1()
        {
            faceClient.BaseUri = new Uri(baseUri);

            // The list of Face attributes to return.
            IList<FaceAttributeType> faceAttributes =
                new FaceAttributeType[]
                {
                    //FaceAttributeType.Gender,
                    FaceAttributeType.Age,
                    FaceAttributeType.Smile,
                    FaceAttributeType.Emotion,
                    //FaceAttributeType.Glasses,
                    //FaceAttributeType.Hair
                };

            // Call the Face API.
            try
            {
                using (Stream imageFileStream = File.OpenRead(ImageFilePath))
                {
                    // The second argument specifies to return the faceId, while
                    // the third argument specifies not to return face landmarks.
                    faceList =
                        await faceClient.Face.DetectWithStreamAsync(
                            imageFileStream, true, false, faceAttributes);

                }

                Assert.IsNotNull(faceList);
            }
            catch (APIErrorException f)
            {
                throw new AssertFailedException("Face API error.", f);
            }

        }
    }
}
