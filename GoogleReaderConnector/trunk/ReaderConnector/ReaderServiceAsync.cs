using System;
using System.Collections.Generic;
using CodeClimber.GoogleReaderConnector.Model;
using CodeClimber.GoogleReaderConnector.Parameters;

namespace CodeClimber.GoogleReaderConnector
{
    public class ReaderServiceAsync : ReaderServiceBase
    {
        public ReaderServiceAsync(IUriBuilder builder, IHttpService httpService) : base(builder, httpService)
        {
        }

        /// <summary>
        /// Same as <see cref="GetFeed"/> but asyncronously
        /// </summary>
        /// <param name="feedUrl">The url of the feed</param>
        /// <param name="parameters">The parameters to configure the feed retrieval</param>
        /// <param name="onSuccess">The callback function that will executed when the call is completed successfully</param>
        /// <param name="onError">The callback function that will executed when an error occurs</param>
        /// <param name="onFinally">The callback function that will always be executed at the end of the call</param>
        public void GetFeed(string feedUrl, ReaderFeedParameters parameters,
            Action<IEnumerable<FeedItem>> onSuccess = null, Action<Exception> onError = null, Action onFinally = null)
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.Feed, feedUrl, parameters);
            ExecGetFeedAsync(requestUrl, onSuccess, onError, onFinally);
        }

        public void GetTag(ItemTag tag, ReaderFeedParameters parameters,
            Action<IEnumerable<FeedItem>> onSuccess = null, Action<Exception> onError = null, Action onFinally = null)
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.Tag, tag, parameters);
            ExecGetFeedAsync(requestUrl, onSuccess, onError, onFinally);
        }

        public void GetLabel(string label, ReaderFeedParameters parameters,
             Action<IEnumerable<FeedItem>> onSuccess = null, Action<Exception> onError = null, Action onFinally = null)
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.Label, label, parameters);
            ExecGetFeedAsync(requestUrl, onSuccess, onError, onFinally);
        }

        public void GetFriend(string userId, Action<Friend> onSuccess = null, Action<Exception> onError = null, Action onFinally = null)
        {
            ReaderFriendParameters parameters = new ReaderFriendParameters { UserId = userId };
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

        public void GetFriends(Action<IEnumerable<Friend>> onSuccess = null, Action<Exception> onError = null, Action onFinally = null)
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.FriendsEdit, new ReaderParametersChoosableOutput());
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

        public void GetUnreadCount(Action<IEnumerable<CountInfo>> onSuccess = null, Action<Exception> onError = null, Action onFinally = null)
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.UnreadCount, new ReaderParametersChoosableOutput());
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


        public void Login(Action<bool> onSuccess = null, Action<Exception> onError = null, Action onFinally = null)
        {
            _httpService.ClientLogin.LoginAsync(onSuccess,onError,onFinally);
        }
    }
}
