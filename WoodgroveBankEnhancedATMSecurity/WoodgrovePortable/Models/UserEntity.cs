
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace WoodgrovePortable.Models
{
    public class UserEntity : TableEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string PersonGroupId { get; set; }
        public string PIN { get; set; }
    }

}
