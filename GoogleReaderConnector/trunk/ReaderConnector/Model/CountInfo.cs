using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
namespace CodeClimber.GoogleReaderConnector.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CountInfo
    {
        [JsonProperty("id")]
        public string FeedId { get; set; }
        [JsonProperty]
        public int Count { get; set; }

        private CountType _type;
        public CountType Type
        {
            get
            {
                if(_type==CountType.Unknown)
                {
                    _type = ParseFeedType();
                }
                return _type;
            }
        }

        private string _userId;
        public string UserId
        {
            get
            {
                if (String.IsNullOrEmpty(_userId))
                {
                    _userId = ParseUserId();
                }
                return _userId;
            }
        }

        private string _feedName;
        public string Name
        {
            get
            {
                if (String.IsNullOrEmpty(_feedName))
                {
                    _feedName = ParseFeedName();
                }
                return _feedName;
            }
        }

        private string ParseFeedName()
        {
            switch (Type)
            {
                case CountType.Unknown:
                    return FeedId;
                case CountType.Feed:
                    return FeedId.Substring(5);
                case CountType.Label:
                    return new Regex(@"user/(\d+)/label/(?<label>.+)").Match(FeedId).Result("${label}");
                case CountType.Shared:
                    return UserId;
                case CountType.All:
                case CountType.AllShared:
                    return "";
                case CountType.State:
                    return new Regex(@"user/(\d+)/state/com.google/(?<state>.+)").Match(FeedId).Result("${state}");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private CountType ParseFeedType()
        {
            if (FeedId.StartsWith("feed"))
                return CountType.Feed;
            if (FeedId.EndsWith("state/com.google/broadcast"))
                return CountType.Shared;
            if (new Regex(@"user/(\d+)/label/(\w+)").IsMatch(FeedId))
                return CountType.Label;
            if (new Regex(@"user/(\d+)/state/com.google/reading-list").IsMatch(FeedId))
                return CountType.All;
            if (new Regex(@"user/(\d+)/state/com.google/broadcast-friends").IsMatch(FeedId))
                return CountType.AllShared;
            if (new Regex(@"user/(\d+)/state/com.google/(\w+)").IsMatch(FeedId))
                return CountType.State;
            return CountType.Unknown;
        }

        private string ParseUserId()
        {
            Match match = new Regex(@"user/(?<userId>\d+)/state/com.google/broadcast").Match(FeedId);
            if (!match.Success)
                return null;
            return match.Result("${userId}");

        }
    }

    public enum CountType
    {
        Unknown,
        Feed,
        Label,
        State,
        All,
        Shared,
        AllShared
    }
}
