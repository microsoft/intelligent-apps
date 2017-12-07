using System;
using System.Collections.Generic;
using System.Text;

namespace WoodgrovePortable.Models
{
    public class Person
    {
        public string name { get; set; }
        public string userData { get; set; }
    }

    public class PersonId
    {
        public string personId { get; set; }
    }
    public class PersonDetails
    {
        public string personId { get; set; }
        public string[] persistedFaceIds { get; set; }
        public string name { get; set; }
        public string userData { get; set; }
    }
}
