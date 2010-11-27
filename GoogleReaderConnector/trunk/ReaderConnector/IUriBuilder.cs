using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace CodeClimber.GoogleReaderConnector
{
    public interface IUriBuilder
    {
        Uri BuildUri(UrlType urlType);
        Uri BuildUri(UrlType urlType, ReaderParameters parameters);
        Uri BuildUri(UrlType urlType, string itemName, ReaderParameters parameters);
        Uri BuildUri(UrlType urlType, StateType state, ReaderParameters parameters);

        Uri GetLoginUri();

        Dictionary<string, string> GetLoginData(string username, string password);
    }
}
