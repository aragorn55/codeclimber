using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeClimber.GoogleReaderConnector.Model;
using System.IO;
using Newtonsoft.Json;
using CodeClimber.GoogleReaderConnector.Services;

namespace CodeClimber.GoogleReaderConnector
{
    public class ReaderService
    {
        private IHttpService _httpService;
        private IUriBuilder _urlBuilder;

        public ReaderService(IUriBuilder builder, IHttpService httpService)
        {
            _urlBuilder = builder;
            _httpService = httpService;
        }

        public IEnumerable<FeedItem> GetFeed(string feedUrl, ReaderParameters parameters, bool authenticate = false)
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.Feed, feedUrl, parameters);

            Feed feed = ParseFeed(requestUrl, authenticate);

            return feed.Items;
        }

        public IEnumerable<FeedItem> GetState(StateType state, ReaderParameters parameters)
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.State, state, parameters);

            Feed feed = ParseFeed(requestUrl, true);

            return feed.Items;
        }

        public IEnumerable<FeedItem> GetTag(string tagName, ReaderParameters parameters)
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.Tag, tagName, parameters);

            Feed feed = ParseFeed(requestUrl, true);

            return feed.Items;
        }

        public Friend GetFriend(string userId)
        {
            ReaderParameters parameters = new ReaderParameters() { UserId = userId };
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.People, parameters);

            IList<Friend> friends = ParseFriends(requestUrl, true);

            return friends[0];
        }

        public IList<Friend> GetFriends()
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.FriendsEdit);

            IList<Friend> friends = ParseFriends(requestUrl, true);

            return friends;
        }

        private IList<Friend> ParseFriends(Uri requestUrl, bool authenticate)
        {
            JsonSerializer serializer = new JsonSerializer();

            FriendList friends;
            using (JsonReader reader = new JsonTextReader(new StreamReader(_httpService.PerformGet(requestUrl, authenticate))))
            {
                friends = serializer.Deserialize<FriendList>(reader);
            }
            return friends.Friends;
        }


        private Feed ParseFeed(Uri requestUrl, bool authenticate)
        {
            JsonSerializer serializer = new JsonSerializer();

            Feed feed;
            using (JsonReader reader = new JsonTextReader(new StreamReader(_httpService.PerformGet(requestUrl, authenticate))))
            {
                feed = serializer.Deserialize<Feed>(reader);
            }
            return feed;
        }

        public IEnumerable<CountInfo> GetUnreadCount()
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.UnreadCount);

            CountInfoList feed = ParseUnreadCount(requestUrl, true);

            return feed.UnreadCounts;
        }

        private CountInfoList ParseUnreadCount(Uri requestUrl, bool authenticate)
        {
            JsonSerializer serializer = new JsonSerializer();

            CountInfoList list;
            using (JsonReader reader = new JsonTextReader(new StreamReader(_httpService.PerformGet(requestUrl, authenticate))))
            {
                list = serializer.Deserialize<CountInfoList>(reader);
            }
            return list;
        }
    }
}
