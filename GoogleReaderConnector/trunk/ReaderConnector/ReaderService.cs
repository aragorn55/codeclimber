using System;
using System.Collections.Generic;
using CodeClimber.GoogleReaderConnector.Model;
using System.IO;
using Newtonsoft.Json;

namespace CodeClimber.GoogleReaderConnector
{
    public class ReaderService
    {
        private readonly IHttpService _httpService;
        private readonly IUriBuilder _urlBuilder;

        public ReaderService(IUriBuilder builder, IHttpService httpService)
        {
            _urlBuilder = builder;
            _httpService = httpService;
        }

        /// <summary>
        /// Returns the list of posts inside a feed
        /// </summary>
        /// <param name="feedUrl">The url of the feed</param>
        /// <param name="parameters">The parameters to configure the feed retrieval</param>
        /// <returns></returns>
        public IEnumerable<FeedItem> GetFeed(string feedUrl, ReaderParameters parameters)
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.Feed, feedUrl, parameters);
            return ExecGetFeed(requestUrl);
        }

        public IEnumerable<FeedItem> GetState(StateType state, ReaderParameters parameters)
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.State, state, parameters);
            return ExecGetFeed(requestUrl);
        }

        public IEnumerable<FeedItem> GetTag(string tagName, ReaderParameters parameters)
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.Tag, tagName, parameters);
            return ExecGetFeed(requestUrl);
        }

        public Friend GetFriend(string userId)
        {
            ReaderParameters parameters = new ReaderParameters { UserId = userId };
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.People, parameters);

            Stream stream = _httpService.PerformGet(requestUrl);
            return ParseResultStream<FriendList>(stream).Friends[0];
        }

        public IList<Friend> GetFriends()
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.FriendsEdit);
            Stream stream = _httpService.PerformGet(requestUrl);
            return ParseResultStream<FriendList>(stream).Friends;
        }

        public IEnumerable<CountInfo> GetUnreadCount()
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.UnreadCount);
            Stream stream = _httpService.PerformGet(requestUrl);
            return ParseResultStream<CountInfoList>(stream).UnreadCounts;
        }

        private IEnumerable<FeedItem> ExecGetFeed(Uri requestUrl)
        {
            Stream stream = _httpService.PerformGet(requestUrl);
            return ParseResultStream<Feed>(stream).Items;
        }

        private static T ParseResultStream<T>(Stream stream) where T : new()
        {
            JsonSerializer serializer = new JsonSerializer();
            T parsed;
            using (JsonReader reader = new JsonTextReader(new StreamReader(stream)))
            {
                parsed = serializer.Deserialize<T>(reader);
            }
            return parsed;
        }
    }
}
