using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using CodeClimber.GoogleReaderConnector.JsonHelpers;

namespace CodeClimber.GoogleReaderConnector.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class FeedItem
    {
        [JsonProperty]
        public String Id { get; set; }
        [JsonProperty("categories")]
        public IList<String> Categories { get;  set; }
        [JsonProperty]
        public String Title { get; set; }
        [JsonProperty]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime Published { get; set; }
        [JsonProperty("updated")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime LastUpdated { get; set; }
        [JsonProperty("Alternate")]
        public IList<AlternativeUrl> Alternative { get;  set; }

        [JsonProperty]
        public FeedSummary Summary { get; set; }

        [JsonProperty("origin")]
        public Blog Blog { get; set; }

        [JsonProperty("via")]
        public IList<SharingPerson> SharedBy { get; set; }

        [JsonProperty]
        public string Author { get; set; }
        [JsonProperty("likingUsers")]
        public IList<User> Likes { get;  set; }
    }

    public class FeedSummary
    {
        public string Content { get; set; }
        public string Direction { get; set; }
    }
}
