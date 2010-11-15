using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace CodeClimber.GoogleReaderConnector.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Feed
    {
        [JsonProperty]
        public string Id { get; set; }
        [JsonProperty]
        public string Title { get; set; }
        [JsonProperty]
        public string Description { get; set; }
        [JsonProperty("Alternate")]
        public IList<AlternativeUrl> Alternative { get; set; }

        public DateTime LastUpdated { get; set; }
        [JsonProperty]
        public IList<FeedItem> Items { get; set; }
    }
}
