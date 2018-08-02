using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using ServiceHelpers;
using Windows.Storage;

namespace IntelligentKioskSample.Azure.Api.UwTests
{
    [TestClass]
    public class ImageAnalyzerTests
    {
        [TestMethod]
        public async Task FaceApi()
        {
            ImageAnalyzer ia = new ImageAnalyzer(await DemoImageBytes());

            await ia.DetectEmotionAsync();

            Assert.IsNotNull(ia.DetectedEmotion);
        }

        [TestMethod]
        public async Task FaceApiEmotion()
        {
            ImageAnalyzer ia = new ImageAnalyzer(await DemoImageBytes());

            await ia.DetectEmotionAsync();

            Assert.IsNotNull(ia.DetectedEmotion);
            Assert.IsTrue(ia.DetectedEmotion.Count > 0);
        }

        [TestMethod]
        public async Task FaceApiEmotionScores()
        {
            ImageAnalyzer ia = new ImageAnalyzer(await DemoImageBytes());

            await ia.DetectEmotionAsync();

            Assert.IsNotNull(ia.DetectedEmotion);
            Assert.IsTrue(ia.DetectedEmotion.Count > 0);
            double emotionsSum = (from j in ia.DetectedEmotion select j.Scores).Sum(x => x.Anger + x.Contempt + x.Disgust + x.Fear + x.Happiness + x.Neutral + x.Sadness + x.Surprise);
            Assert.IsTrue(emotionsSum > 0);
        }


        private async Task<byte[]> DemoImageBytes() {
            StorageFolder InstallationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            StorageFile file = await InstallationFolder.GetFileAsync(FaceApiTests.DEMO_IMG_FILE);

            using (Stream imageFileStream = await file.OpenStreamForReadAsync())
            {
                byte[] buffer = new byte[imageFileStream.Length];
                await imageFileStream.ReadAsync(buffer, 0, (int)imageFileStream.Length);

                return buffer;
            }
        }
    }
}
