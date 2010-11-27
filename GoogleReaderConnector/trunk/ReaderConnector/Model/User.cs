using Newtonsoft.Json;

namespace CodeClimber.GoogleReaderConnector.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class User
    {
        [JsonProperty("userId")]
        public string Id { get; set; }
    }
}
