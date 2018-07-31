using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceHelpers.Data
{
    public class EmotionData
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
        public float Anger { get; set; }
        public float Contempt { get; set; }
        public float Disgust { get; set; }
        public float Fear { get; set; }
        public float Happiness { get; set; }
        public float Neutral { get; set; }
        public float Sadness { get; set; }
        public float Surprise { get; set; }

        //public IEnumerable<KeyValuePair<string, float>> ToRankedList();
    }
}
