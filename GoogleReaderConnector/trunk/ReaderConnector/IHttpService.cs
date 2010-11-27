using System;
using System.IO;
using System.Collections.Specialized;

namespace CodeClimber.GoogleReaderConnector
{
    public interface IHttpService
    {
        IClientLoginService ClientLogin { get; set; }
        Stream PerformGet(Uri url);
        string PerformPost(Uri url, NameValueCollection values);

        void PerformGetAsync(Uri requestUrl, Action<Stream> onSuccess = null, Action<Exception> onError = null, Action onFinally = null);
        void PerformPostAsync(Uri url, NameValueCollection values, Action<string> onSuccess = null, Action<Exception> onError = null, Action onFinally = null);
    }
}
