using System;
using System.Collections.Generic;
using System.IO;
using CodeClimber.GoogleReaderConnector.Model;
using Newtonsoft.Json;

namespace CodeClimber.GoogleReaderConnector
{
    public class ReaderServiceAsync
    {
       private readonly IHttpService _httpService;
        private readonly IUriBuilder _urlBuilder;

        public ReaderServiceAsync(IUriBuilder builder, IHttpService httpService)
        {
            _urlBuilder = builder;
            _httpService = httpService;
        }

        /// <summary>
        /// Same as <see cref="GetFeed"/> but asyncronously
        /// </summary>
        /// <param name="feedUrl">The url of the feed</param>
        /// <param name="parameters">The parameters to configure the feed retrieval</param>
        /// <param name="onSuccess">The callback function that will executed when the call is completed successfully</param>
        /// <param name="onError">The callback function that will executed when an error occurs</param>
        /// <param name="onFinally">The callback function that will always be executed at the end of the call</param>
        public void GetFeedAsync(string feedUrl, ReaderParameters parameters,
            Action<IEnumerable<FeedItem>> onSuccess = null, Action<Exception> onError = null, Action onFinally = null)
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.Feed, feedUrl, parameters);
            ExecGetFeedAsync(requestUrl, onSuccess, onError, onFinally);
        }

        public void GetStateAsync(StateType state, ReaderParameters parameters,
            Action<IEnumerable<FeedItem>> onSuccess = null, Action<Exception> onError = null, Action onFinally = null)
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.State, state, parameters);
            ExecGetFeedAsync(requestUrl, onSuccess, onError, onFinally);
        }

        public void GetTagAsync(string tagName, ReaderParameters parameters,
             Action<IEnumerable<FeedItem>> onSuccess = null, Action<Exception> onError = null, Action onFinally = null)
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.Tag, tagName, parameters);
            ExecGetFeedAsync(requestUrl, onSuccess, onError, onFinally);
        }

        public void GetFriendAsync(string userId, Action<Friend> onSuccess = null, Action<Exception> onError = null, Action onFinally = null)
        {
            ReaderParameters parameters = new ReaderParameters { UserId = userId };
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.People, parameters);

            _httpService.PerformGetAsync(requestUrl,
                                         stream =>
                                         {
                                             FriendList friends = ParseResultStream<FriendList>(stream);

                                             if (onSuccess != null)
                                             {
                                                 onSuccess(friends.Friends[0]);
                                             }
                                         },
                onError, onFinally);
        }

        public void GetFriendsAsync(Action<IEnumerable<Friend>> onSuccess = null, Action<Exception> onError = null, Action onFinally = null)
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.FriendsEdit);
            _httpService.PerformGetAsync(requestUrl,
                                         stream =>
                                         {
                                             FriendList friends = ParseResultStream<FriendList>(stream);

                                             if (onSuccess != null)
                                             {
                                                 onSuccess(friends.Friends);
                                             }
                                         },
                onError, onFinally);
        }

        public void GetUnreadCountAsync(Action<IEnumerable<CountInfo>> onSuccess = null, Action<Exception> onError = null, Action onFinally = null)
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.UnreadCount);
            _httpService.PerformGetAsync(requestUrl,
                                         stream =>
                                         {
                                             CountInfoList countInfoList = ParseResultStream<CountInfoList>(stream);

                                             if (onSuccess != null)
                                             {
                                                 onSuccess(countInfoList.UnreadCounts);
                                             }
                                         },
                onError, onFinally);
        }

        private void ExecGetFeedAsync(Uri requestUrl, Action<IEnumerable<FeedItem>> onSuccess = null, Action<Exception> onError = null, Action onFinally = null)
        {
            _httpService.PerformGetAsync(requestUrl,
                                         stream =>
                                         {
                                             Feed feed = ParseResultStream<Feed>(stream);

                                             if (onSuccess != null)
                                             {
                                                 onSuccess(feed.Items);
                                             }
                                         },
                onError, onFinally);
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
