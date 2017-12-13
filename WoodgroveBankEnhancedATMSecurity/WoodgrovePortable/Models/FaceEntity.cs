
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace WoodgrovePortable.Models
{
    public class FaceEntity : TableEntity
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string ImageUrl { get; set; }
        public string PersonId { get; set; }
        public string PersistedFaceID { get; set; }
    }

}
