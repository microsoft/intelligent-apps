using System;
using System.Collections.Generic;
using System.Text;

namespace WoodgrovePortable.Models
{
    public class PersonFace
    {
        public string url { get; set; }
    }
    public class PersistedFace
    {
        public string persistedFaceId { get; set; }
    }
    public class DetectFace
    {
        public FaceClass[] FaceData { get; set; }

        public class FaceClass
        {
            public string faceId { get; set; }
            public Facerectangle faceRectangle { get; set; }
            public Facelandmarks faceLandmarks { get; set; }
            public Faceattributes faceAttributes { get; set; }
            public string glasses { get; set; }
            public Headpose headPose { get; set; }
            public Emotion emotion { get; set; }
            public Hair hair { get; set; }
            public Makeup makeup { get; set; }
            public Occlusion occlusion { get; set; }
            public Accessory[] accessories { get; set; }
            public Blur blur { get; set; }
            public Exposure exposure { get; set; }
            public Noise noise { get; set; }
        }

        public class Facerectangle
        {
            public int width { get; set; }
            public int height { get; set; }
            public int left { get; set; }
            public int top { get; set; }
        }

        public class Facelandmarks
        {
            public Pupilleft pupilLeft { get; set; }
            public Pupilright pupilRight { get; set; }
            public Nosetip noseTip { get; set; }
            public Mouthleft mouthLeft { get; set; }
            public Mouthright mouthRight { get; set; }
            public Eyebrowleftouter eyebrowLeftOuter { get; set; }
            public Eyebrowleftinner eyebrowLeftInner { get; set; }
            public Eyeleftouter eyeLeftOuter { get; set; }
            public Eyelefttop eyeLeftTop { get; set; }
            public Eyeleftbottom eyeLeftBottom { get; set; }
            public Eyeleftinner eyeLeftInner { get; set; }
            public Eyebrowrightinner eyebrowRightInner { get; set; }
            public Eyebrowrightouter eyebrowRightOuter { get; set; }
            public Eyerightinner eyeRightInner { get; set; }
            public Eyerighttop eyeRightTop { get; set; }
            public Eyerightbottom eyeRightBottom { get; set; }
            public Eyerightouter eyeRightOuter { get; set; }
            public Noserootleft noseRootLeft { get; set; }
            public Noserootright noseRootRight { get; set; }
            public Noseleftalartop noseLeftAlarTop { get; set; }
            public Noserightalartop noseRightAlarTop { get; set; }
            public Noseleftalarouttip noseLeftAlarOutTip { get; set; }
            public Noserightalarouttip noseRightAlarOutTip { get; set; }
            public Upperliptop upperLipTop { get; set; }
            public Upperlipbottom upperLipBottom { get; set; }
            public Underliptop underLipTop { get; set; }
            public Underlipbottom underLipBottom { get; set; }
        }

        public class Pupilleft
        {
            public float x { get; set; }
            public float y { get; set; }
        }

        public class Pupilright
        {
            public float x { get; set; }
            public float y { get; set; }
        }

        public class Nosetip
        {
            public float x { get; set; }
            public float y { get; set; }
        }

        public class Mouthleft
        {
            public float x { get; set; }
            public float y { get; set; }
        }

        public class Mouthright
        {
            public float x { get; set; }
            public float y { get; set; }
        }

        public class Eyebrowleftouter
        {
            public float x { get; set; }
            public float y { get; set; }
        }

        public class Eyebrowleftinner
        {
            public float x { get; set; }
            public float y { get; set; }
        }

        public class Eyeleftouter
        {
            public float x { get; set; }
            public float y { get; set; }
        }

        public class Eyelefttop
        {
            public float x { get; set; }
            public float y { get; set; }
        }

        public class Eyeleftbottom
        {
            public float x { get; set; }
            public float y { get; set; }
        }

        public class Eyeleftinner
        {
            public float x { get; set; }
            public float y { get; set; }
        }

        public class Eyebrowrightinner
        {
            public float x { get; set; }
            public float y { get; set; }
        }

        public class Eyebrowrightouter
        {
            public float x { get; set; }
            public float y { get; set; }
        }

        public class Eyerightinner
        {
            public float x { get; set; }
            public float y { get; set; }
        }

        public class Eyerighttop
        {
            public float x { get; set; }
            public float y { get; set; }
        }

        public class Eyerightbottom
        {
            public float x { get; set; }
            public float y { get; set; }
        }

        public class Eyerightouter
        {
            public float x { get; set; }
            public float y { get; set; }
        }

        public class Noserootleft
        {
            public float x { get; set; }
            public float y { get; set; }
        }

        public class Noserootright
        {
            public float x { get; set; }
            public float y { get; set; }
        }

        public class Noseleftalartop
        {
            public float x { get; set; }
            public float y { get; set; }
        }

        public class Noserightalartop
        {
            public float x { get; set; }
            public float y { get; set; }
        }

        public class Noseleftalarouttip
        {
            public float x { get; set; }
            public float y { get; set; }
        }

        public class Noserightalarouttip
        {
            public float x { get; set; }
            public float y { get; set; }
        }

        public class Upperliptop
        {
            public float x { get; set; }
            public float y { get; set; }
        }

        public class Upperlipbottom
        {
            public float x { get; set; }
            public float y { get; set; }
        }

        public class Underliptop
        {
            public float x { get; set; }
            public float y { get; set; }
        }

        public class Underlipbottom
        {
            public float x { get; set; }
            public float y { get; set; }
        }

        public class Faceattributes
        {
            public float age { get; set; }
            public string gender { get; set; }
            public float smile { get; set; }
            public Facialhair facialHair { get; set; }
        }

        public class Facialhair
        {
            public float moustache { get; set; }
            public float beard { get; set; }
            public float sideburns { get; set; }
        }

        public class Headpose
        {
            public float roll { get; set; }
            public int yaw { get; set; }
            public int pitch { get; set; }
        }

        public class Emotion
        {
            public float anger { get; set; }
            public int contempt { get; set; }
            public float disgust { get; set; }
            public float fear { get; set; }
            public float happiness { get; set; }
            public float neutral { get; set; }
            public int sadness { get; set; }
            public float surprise { get; set; }
        }

        public class Hair
        {
            public float bald { get; set; }
            public bool invisible { get; set; }
            public Haircolor[] hairColor { get; set; }
        }

        public class Haircolor
        {
            public string color { get; set; }
            public float confidence { get; set; }
        }

        public class Makeup
        {
            public bool eyeMakeup { get; set; }
            public bool lipMakeup { get; set; }
        }

        public class Occlusion
        {
            public bool foreheadOccluded { get; set; }
            public bool eyeOccluded { get; set; }
            public bool mouthOccluded { get; set; }
        }

        public class Blur
        {
            public string blurLevel { get; set; }
            public float value { get; set; }
        }

        public class Exposure
        {
            public string exposureLevel { get; set; }
            public float value { get; set; }
        }

        public class Noise
        {
            public string noiseLevel { get; set; }
            public float value { get; set; }
        }

        public class Accessory
        {
            public string type { get; set; }
            public float confidence { get; set; }
        }

    }
    public class IdenfityFaceRequestModel
    {
        public string personGroupId { get; set; }
        public string[] faceIds { get; set; }
        public int maxNumOfCandidatesReturned { get; set; }
        public float confidenceThreshold { get; set; }
    }
    public class IdentifyFaceResponseModel
    {
        public string faceId { get; set; }
        public Candidate[] candidates { get; set; }
    }

    public class Candidate
    {
        public string personId { get; set; }
        public float confidence { get; set; }
    }
}
