using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeClimber.GoogleReaderConnector
{
    public interface IUriBuilder
    {
        Uri BuildUri(UrlType urlType, string url, ReaderParameters parameters);

        Uri BuildUri(UrlType urlType, StateType state, ReaderParameters parameters);

        Uri GetLoginUri();

        string GetLoginData(string Username, string Password);
    }
}
