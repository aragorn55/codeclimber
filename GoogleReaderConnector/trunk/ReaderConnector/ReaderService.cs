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


        public IEnumerable<FeedItem> GetFeedContent(string feedUrl, ReaderParameters parameters, bool authenticate = false)
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.Feed, feedUrl, parameters);

            Feed feed = GetFeed(requestUrl, authenticate);

            return feed.Items;
        }

        public IEnumerable<FeedItem> GetState(StateType state, ReaderParameters parameters,bool authenticate)
        {
            Uri requestUrl = _urlBuilder.BuildUri(UrlType.State, state, parameters);

            Feed feed = GetFeed(requestUrl, authenticate);

            return feed.Items;
        }

        private Feed GetFeed(Uri requestUrl, bool authenticate)
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
