using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace CodeClimber.GoogleReaderConnector.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SharingPerson
    {
        [JsonProperty]
        public string Title { get; set; }
        [JsonProperty("href")]
        public Uri PublicSharedFeed { get; set; }

        private string _userId;
        public string UserId
        {
            get
            {
                if (String.IsNullOrEmpty(_userId))
                {
                    _userId = ParseUserIdFromSharedFeed();
                }
                return _userId;
            }
        }

        private string ParseUserIdFromSharedFeed()
        {
            return new Regex(@"http://www.google.com/reader/public/atom/user/(?<userId>\d+)/state/com.google/broadcast").Match(PublicSharedFeed.AbsoluteUri).Result("${userId}");
        }
    }
}
