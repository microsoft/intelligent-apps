using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ServiceHelpers.Tests
{
    [TestClass]
    public class ImageAnalyzerTests
    {
        [TestMethod]
        public async Task FaceApi()
        {
            ImageAnalyzer ia = new ImageAnalyzer(File.ReadAllBytes(@"C:\Dev\intelligent-apps\ServiceHelpers.Tests\Assets\demo_face_api.png"));// "https://docs.microsoft.com/en-us/azure/cognitive-services/face/images/getting-started-cs-ui.png");

            await ia.DetectEmotionAsync();

            Assert.IsNotNull(ia.DetectedEmotion);
        }

        [TestMethod]
        public async Task FaceApiEmotion()
        {
            ImageAnalyzer ia = new ImageAnalyzer(File.ReadAllBytes(@"C:\Dev\intelligent-apps\ServiceHelpers.Tests\Assets\demo_face_api.png"));// "https://docs.microsoft.com/en-us/azure/cognitive-services/face/images/getting-started-cs-ui.png");

            await ia.DetectEmotionAsync();

            Assert.IsNotNull(ia.DetectedEmotion);
            Assert.IsTrue(ia.DetectedEmotion.Count > 0);
        }

        [TestMethod]
        public async Task FaceApiEmotionScores()
        {
            ImageAnalyzer ia = new ImageAnalyzer(File.ReadAllBytes(@"C:\Dev\intelligent-apps\ServiceHelpers.Tests\Assets\demo_face_api.png"));// "https://docs.microsoft.com/en-us/azure/cognitive-services/face/images/getting-started-cs-ui.png");

            await ia.DetectEmotionAsync();

            Assert.IsNotNull(ia.DetectedEmotion);
            Assert.IsTrue(ia.DetectedEmotion.Count > 0);
            double emotionsSum = (from j in ia.DetectedEmotion select j.Scores).Sum(x => x.Anger + x.Contempt + x.Disgust + x.Fear + x.Happiness + x.Neutral + x.Sadness + x.Surprise);
            Assert.IsTrue(emotionsSum > 0);
        }
    }
}
