using System;
using Newtonsoft.Json;

namespace CodeClimber.GoogleReaderConnector.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AlternativeUrl
    {
        [JsonProperty("href")]
        public Uri Url { get; set; }
        [JsonProperty("type")]
        public string MimeType { get; set; }
    }
}
