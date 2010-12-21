using System;
using System.Collections.Generic;
using CodeClimber.GoogleReaderConnector.Parameters;

namespace CodeClimber.GoogleReaderConnector
{
    public interface IUriBuilder
    {
        Uri BuildUri(UrlType urlType);
        Uri BuildUri(UrlType urlType, ReaderParametersBase parameters);
        Uri BuildUri(UrlType urlType, string itemName, ReaderParametersBase parameters);
        Uri BuildUri(UrlType urlType, ItemTag state, ReaderParametersBase parameters);

        Uri GetLoginUri();
        Uri GetTokenUri();

        Dictionary<string, string> GetLoginData(string username, string password);
        Dictionary<string, string> GetItemEditData(string token, string feedId, string itemId, ItemTag addTag, ItemTag removeTag, ItemAction action);

        Uri GetPhotoUrl(string photoUrl);
    }
}
