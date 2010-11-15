using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace CodeClimber.GoogleReaderConnector.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class User
    {
        public string Id { get; set; }
    }
}
