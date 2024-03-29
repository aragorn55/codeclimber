﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
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
        public FeedContent Summary { get; set; }

        [JsonProperty]
        public FeedContent Content { get; set; }

        [JsonProperty("origin")]
        public Blog Blog { get; set; }

        [JsonProperty("via")]
        public IList<SharingPerson> SharedBy { get; set; }

        [JsonProperty]
        public string Author { get; set; }
        [JsonProperty("likingUsers")]
        public IList<User> Likes { get;  set; }
        
        public FeedContent GetContent()
        {
            return Content ?? Summary;
        }
    }

    public class FeedContent
    {
        public string Content { get; set; }
        public string Direction { get; set; }
    }
}
