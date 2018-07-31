using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceHelpers.Data
{
    public class FaceEmotionData
    {
        public Rectangle FaceRectangle { get; set; }
        public EmotionScores Scores { get; set; }
    }

    public class Rectangle
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class EmotionScores
    {
        public double Anger { get; set; }
        public double Contempt { get; set; }
        public double Disgust { get; set; }
        public double Fear { get; set; }
        public double Happiness { get; set; }
        public double Neutral { get; set; }
        public double Sadness { get; set; }
        public double Surprise { get; set; }

        //public IEnumerable<KeyValuePair<string, float>> ToRankedList();
    }
}
