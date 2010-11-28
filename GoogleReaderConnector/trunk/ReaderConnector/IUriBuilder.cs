using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using CodeClimber.GoogleReaderConnector.Parameters;

namespace CodeClimber.GoogleReaderConnector
{
    public interface IUriBuilder
    {
        Uri BuildUri(UrlType urlType, ReaderParametersBase parameters);
        Uri BuildUri(UrlType urlType, string itemName, ReaderParametersBase parameters);
        Uri BuildUri(UrlType urlType, StateType state, ReaderParametersBase parameters);

        Uri GetLoginUri();

        Dictionary<string, string> GetLoginData(string username, string password);

        Uri GetPhotoUrl(string photoUrl);
    }
}
