using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdatumTaxCorpKnowledgeService.Models
{
    public class QnaMakerQuestion
    {
        [JsonProperty(PropertyName = "question")]
        public string Question { get; set; }
    }
}