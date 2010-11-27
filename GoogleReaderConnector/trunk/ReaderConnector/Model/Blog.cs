using System;
using Newtonsoft.Json;

namespace CodeClimber.GoogleReaderConnector.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Blog
    {
        [JsonProperty("streamId")]
        public string Id { get; set; }
        [JsonProperty]
        public string Title { get; set; }
        [JsonProperty("href")]
        public Uri Url { get; set; }
    }
}
