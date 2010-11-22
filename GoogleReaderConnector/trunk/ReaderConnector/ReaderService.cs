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
            return ExecGetFeed(requestUrl, authenticate);
        }

        public IEnumerable<FeedItem> GetState(StateType state, ReaderParameters parameters)
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.State, state, parameters);
            return ExecGetFeed(requestUrl, true);
        }

        public IEnumerable<FeedItem> GetTag(string tagName, ReaderParameters parameters)
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.Tag, tagName, parameters);
            return ExecGetFeed(requestUrl, true);
        }

        public Friend GetFriend(string userId)
        {
            ReaderParameters parameters = new ReaderParameters() { UserId = userId };

            Uri requestUrl = _urlBuilder.BuildUri(UrlType.People, parameters);
            Stream stream = _httpService.PerformGet(requestUrl, true);
            return ParseResultStream<FriendList>(stream).Friends[0];
        }

        public IList<Friend> GetFriends()
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.FriendsEdit);
            Stream stream = _httpService.PerformGet(requestUrl, true);
            return ParseResultStream<FriendList>(stream).Friends;
        }

        public IEnumerable<CountInfo> GetUnreadCount()
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.UnreadCount);
            Stream stream = _httpService.PerformGet(requestUrl, true);
            return ParseResultStream<CountInfoList>(stream).UnreadCounts;
        }

        public void GetFeedAsync(string feedUrl, ReaderParameters parameters, 
             Action<IEnumerable<FeedItem>> onGetFeedCompleted = null, Action<Exception> onError = null, Action onFinally = null, bool authenticate = false)
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.Feed, feedUrl, parameters);
            ExecGetFeedAsync(requestUrl,authenticate,onGetFeedCompleted,onError,onFinally);
        }

        private void ExecGetFeedAsync(Uri requestUrl, bool authenticate, Action<IEnumerable<FeedItem>> onGetFeedCompleted = null, Action<Exception> onError = null, Action onFinally = null)
        {
            _httpService.PerformGetAsync(requestUrl, authenticate,
                delegate(Stream stream)
                {
                    Feed feed = ParseResultStream<Feed>(stream);

                    if (onGetFeedCompleted != null)
                    {
                        onGetFeedCompleted(feed.Items);
                    }
                },
                onError, onFinally);
        }

        private IEnumerable<FeedItem> ExecGetFeed(Uri requestUrl, bool authenticate)
        {
            Stream stream = _httpService.PerformGet(requestUrl, authenticate);
            return ParseResultStream<Feed>(stream).Items;
        }

        private T ParseResultStream<T>(Stream stream) where T : new()
        {
            JsonSerializer serializer = new JsonSerializer();
            T parsed = new T();
            using (JsonReader reader = new JsonTextReader(new StreamReader(stream)))
            {
                parsed = serializer.Deserialize<T>(reader);
            }
            return parsed;
        }

        public void GetTagAsync(string tagName, ReaderParameters parameters,
             Action<IEnumerable<FeedItem>> onGetFeedCompleted = null, Action<Exception> onError = null, Action onFinally = null)
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.Tag, tagName, parameters);
            ExecGetFeedAsync(requestUrl, true, onGetFeedCompleted, onError, onFinally);
        }
    }
}
