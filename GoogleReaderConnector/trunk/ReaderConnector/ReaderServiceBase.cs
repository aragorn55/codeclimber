using System;
using System.Collections.Generic;
using System.IO;
using CodeClimber.GoogleReaderConnector.Model;
using Newtonsoft.Json;

namespace CodeClimber.GoogleReaderConnector
{
    public abstract class ReaderServiceBase
    {
        protected IHttpService _httpService;
        protected IUriBuilder _urlBuilder;

        public ReaderServiceBase(IUriBuilder builder, IHttpService httpService)
        {
            _urlBuilder = builder;
            _httpService = httpService;
        }

        public Uri GetFriendPhotoUrl(Friend friend)
        {
            return _urlBuilder.GetPhotoUrl(friend.PhotoUrl);
        }

        protected static T ParseResultStream<T>(Stream stream) where T : new()
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