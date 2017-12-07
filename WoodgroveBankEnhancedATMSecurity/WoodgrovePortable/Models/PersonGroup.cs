using System;
using System.Collections.Generic;
using System.Text;

namespace WoodgrovePortable.Models
{
    public class PersonGroup
    {
        public string name { get; set; }
        public string userData { get; set; }
    }

    public class PersonGroupTrainingStatus
    {
        public string status { get; set; }
        public string createdDateTime { get; set; }
        public string lastActionDateTime { get; set; }
        public object message { get; set; }
    }
    public class PersonGroupDetails
    {
        public string personGroupId { get; set; }
        public string name { get; set; }
        public string userData { get; set; }
    }
}
