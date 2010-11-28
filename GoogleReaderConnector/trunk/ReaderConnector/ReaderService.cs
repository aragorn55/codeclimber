using System;
using System.Collections.Generic;
using CodeClimber.GoogleReaderConnector.Model;
using System.IO;
using CodeClimber.GoogleReaderConnector.Parameters;
using Newtonsoft.Json;

namespace CodeClimber.GoogleReaderConnector
{
    public class ReaderService: ReaderServiceBase
    {
        public ReaderService(IUriBuilder builder, IHttpService httpService)
            : base(builder, httpService)
        {
        }

        /// <summary>
        /// Returns the list of posts inside a feed
        /// </summary>
        /// <param name="feedUrl">The url of the feed</param>
        /// <param name="parameters">The parameters to configure the feed retrieval</param>
        /// <returns></returns>
        public IEnumerable<FeedItem> GetFeed(string feedUrl, ReaderFeedParameters parameters)
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.Feed, feedUrl, parameters);
            return ExecGetFeed(requestUrl);
        }

        public IEnumerable<FeedItem> GetState(StateType state, ReaderFeedParameters parameters)
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.State, state, parameters);
            return ExecGetFeed(requestUrl);
        }

        public IEnumerable<FeedItem> GetTag(string tagName, ReaderFeedParameters parameters)
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.Tag, tagName, parameters);
            return ExecGetFeed(requestUrl);
        }

        public Friend GetFriend(string userId)
        {
            ReaderFriendParameters parameters = new ReaderFriendParameters { UserId = userId };
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.People, parameters);

            Stream stream = _httpService.PerformGet(requestUrl);
            return ParseResultStream<FriendList>(stream).Friends[0];
        }

        public IList<Friend> GetFriends()
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.FriendsEdit, new ReaderParametersChoosableOutput());
            Stream stream = _httpService.PerformGet(requestUrl);
            return ParseResultStream<FriendList>(stream).Friends;
        }

        public IEnumerable<CountInfo> GetUnreadCount()
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.UnreadCount, new ReaderParametersChoosableOutput());
            Stream stream = _httpService.PerformGet(requestUrl);
            return ParseResultStream<CountInfoList>(stream).UnreadCounts;
        }

        private IEnumerable<FeedItem> ExecGetFeed(Uri requestUrl)
        {
            Stream stream = _httpService.PerformGet(requestUrl);
            return ParseResultStream<Feed>(stream).Items;
        }
    }
}
