﻿using System;
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

        public void GetFeedAsync(string feedUrl, ReaderParameters parameters, 
            Action<IEnumerable<FeedItem>> onGetFeedCompleted = null, Action<Exception> onError = null, Action onFinally = null, bool authenticate = false)
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.Feed, feedUrl, parameters);
            ExecGetFeedAsync(requestUrl, authenticate, onGetFeedCompleted, onError, onFinally);
        }

        public IEnumerable<FeedItem> GetState(StateType state, ReaderParameters parameters)
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.State, state, parameters);
            return ExecGetFeed(requestUrl, true);
        }

        public void GetStateAsync(StateType state, ReaderParameters parameters,
            Action<IEnumerable<FeedItem>> onGetFeedCompleted = null, Action<Exception> onError = null, Action onFinally = null)
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.State, state, parameters);
            ExecGetFeedAsync(requestUrl, true, onGetFeedCompleted, onError, onFinally);
        }

        public IEnumerable<FeedItem> GetTag(string tagName, ReaderParameters parameters)
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.Tag, tagName, parameters);
            return ExecGetFeed(requestUrl, true);
        }

        public void GetTagAsync(string tagName, ReaderParameters parameters,
             Action<IEnumerable<FeedItem>> onGetFeedCompleted = null, Action<Exception> onError = null, Action onFinally = null)
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.Tag, tagName, parameters);
            ExecGetFeedAsync(requestUrl, true, onGetFeedCompleted, onError, onFinally);
        }

        public Friend GetFriend(string userId)
        {
            ReaderParameters parameters = new ReaderParameters() { UserId = userId };
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.People, parameters);

            Stream stream = _httpService.PerformGet(requestUrl, true);
            return ParseResultStream<FriendList>(stream).Friends[0];
        }

        public void GetFriendAsync(string userId, Action<Friend> onGetFriendCompleted = null, Action<Exception> onError = null, Action onFinally = null)
        {
            ReaderParameters parameters = new ReaderParameters() { UserId = userId };
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.People, parameters);

            _httpService.PerformGetAsync(requestUrl, true,
                delegate(Stream stream)
                {
                    FriendList friends = ParseResultStream<FriendList>(stream);

                    if (onGetFriendCompleted != null)
                    {
                        onGetFriendCompleted(friends.Friends[0]);
                    }
                },
                onError, onFinally);
        }

        public IList<Friend> GetFriends()
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.FriendsEdit);
            Stream stream = _httpService.PerformGet(requestUrl, true);
            return ParseResultStream<FriendList>(stream).Friends;
        }

        public void GetFriendsAsync(Action<IEnumerable<Friend>> onGetFriendsCompleted = null, Action<Exception> onError = null, Action onFinally = null)
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.FriendsEdit);
            _httpService.PerformGetAsync(requestUrl, true,
                delegate(Stream stream)
                {
                    FriendList friends = ParseResultStream<FriendList>(stream);

                    if (onGetFriendsCompleted != null)
                    {
                        onGetFriendsCompleted(friends.Friends);
                    }
                },
                onError, onFinally);
        }

        public IEnumerable<CountInfo> GetUnreadCount()
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.UnreadCount);
            Stream stream = _httpService.PerformGet(requestUrl, true);
            return ParseResultStream<CountInfoList>(stream).UnreadCounts;
        }

        public void GetUnreadCountAsync(Action<IEnumerable<CountInfo>> onGetFriendsCompleted = null, Action<Exception> onError = null, Action onFinally = null)
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.UnreadCount);
            _httpService.PerformGetAsync(requestUrl, true,
                delegate(Stream stream)
                {
                    CountInfoList countInfoList = ParseResultStream<CountInfoList>(stream);

                    if (onGetFriendsCompleted != null)
                    {
                        onGetFriendsCompleted(countInfoList.UnreadCounts);
                    }
                },
                onError, onFinally);
        }

        private IEnumerable<FeedItem> ExecGetFeed(Uri requestUrl, bool authenticate)
        {
            Stream stream = _httpService.PerformGet(requestUrl, authenticate);
            return ParseResultStream<Feed>(stream).Items;
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

        private T ParseResultStream<T>(Stream stream) where T : new()
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
