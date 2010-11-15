using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace CodeClimber.GoogleReaderConnector.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class FeedItem
    {
        [JsonProperty]
        public String Id { get; set; }
        public IList<String> Categories { get;  set; }
        public String Title { get; set; }
        //public DateTime Published { get; set; }
        //public DateTime LastUpdated { get; set; }
        [JsonProperty("Alternate")]
        public IList<AlternativeUrl> Alternative { get;  set; }
        public string Summary { get; set; }
        [JsonProperty("direction")]
        public string SummaryDirection { get; set; }
        public string Author { get; set; }
        public IList<User> Likes { get;  set; }
    }
}
