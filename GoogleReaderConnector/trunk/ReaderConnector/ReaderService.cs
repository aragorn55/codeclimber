using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeClimber.GoogleReaderConnector.Model;
using System.IO;
using Newtonsoft.Json;
using CodeClimber.GoogleReaderConnector.Services;

namespace CodeClimber.GoogleReaderConnector
{
    public class ReaderService : IDisposable
    {
        
        private IHttpService _httpService;
        private IUriBuilder _urlBuilder;

        public ReaderService(IUriBuilder builder, IHttpService httpService)
        {
            _urlBuilder = builder;
            _httpService = httpService;
        }

        public void Dispose()
        {
            if (_httpService != null)
                _httpService.Dispose();
        }


        public IEnumerable<FeedItem> GetFeed(string feedUrl, ReaderParameters parameters, bool authenticate = false)
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.Feed, feedUrl, parameters);

            Feed feed = ParseFeed(requestUrl, authenticate);

            return feed.Items;
        }

        public IEnumerable<FeedItem> GetState(StateType state, ReaderParameters parameters,bool authenticate)
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.State, state, parameters);

            Feed feed = ParseFeed(requestUrl, authenticate);

            return feed.Items;
        }

        public IEnumerable<FeedItem> GetTag(string tagName, ReaderParameters parameters, bool authenticate)
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.Tag, tagName, parameters);

            Feed feed = ParseFeed(requestUrl, authenticate);

            return feed.Items;
        }

        private Feed ParseFeed(Uri requestUrl, bool authenticate)
        {
            JsonSerializer serializer = new JsonSerializer();

            Feed feed;
            using (JsonReader reader = new JsonTextReader(new StreamReader(_httpService.PerformGet(requestUrl, authenticate).GetResponseStream())))
            {
                feed = serializer.Deserialize<Feed>(reader);
            }
            return feed;
        }


    }
}
