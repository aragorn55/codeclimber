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

        public IEnumerable<FeedItem> GetTag(ItemTag tag, ReaderFeedParameters parameters)
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.Tag, tag, parameters);
            return ExecGetFeed(requestUrl);
        }

        public IEnumerable<FeedItem> GetLabel(string label, ReaderFeedParameters parameters)
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.Label, label, parameters);
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

        public bool KeepItemUnread(string feedId, string itemId)
        {
            if (!AddTagToItem(feedId, itemId, ItemTag.TrackingKeptUnread)) return false;
            if (!EditItem(feedId, itemId, ItemTag.KeptUnread, ItemTag.Read, ItemAction.AddAndRemove)) return false;
            return true;
        }

        public bool MarkItemRead(string feedId, string itemId)
        {
            if (!EditItem(feedId, itemId, ItemTag.Read, ItemTag.KeptUnread, ItemAction.AddAndRemove)) return false;
            return true;
        }

        public bool AddTagToItem(string feedId, string itemId, ItemTag tag)
        {
            return EditItem(feedId, itemId, tag, ItemTag.None, ItemAction.Add);
        }

        public bool RemoveTagFromItem(string feedId, string itemId, ItemTag tag)
        {
            return EditItem(feedId, itemId, ItemTag.None, tag, ItemAction.Remove);
        }

        public bool EditItem(string feedId, string itemId, ItemTag addTag, ItemTag removeTag, ItemAction action)
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.ItemEdit);
            string token = GetToken();
            Dictionary<string, string> postData = _urlBuilder.GetItemEditData(token, feedId, itemId, addTag, removeTag, action);
            var result = _httpService.PerformPost(requestUrl, postData);
            if(result.Equals("OK"))
                return true;
            return false;
        }

        private string GetToken()
        {
            Uri requestUrl = _urlBuilder.GetTokenUri();
            Stream stream = _httpService.PerformGet(requestUrl);
            StreamReader r = new StreamReader(stream);
            return r.ReadToEnd();
        }

        private IEnumerable<FeedItem> ExecGetFeed(Uri requestUrl)
        {
            Stream stream = _httpService.PerformGet(requestUrl);
            return ParseResultStream<Feed>(stream).Items;
        }
    }
}
