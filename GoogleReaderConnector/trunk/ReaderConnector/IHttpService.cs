using System;
using System.Collections.Generic;
using System.IO;
using System.Collections.Specialized;

namespace CodeClimber.GoogleReaderConnector
{
    public interface IHttpService
    {
        IClientLoginService ClientLogin { get; set; }
#if !WINDOWS_PHONE
        Stream PerformGet(Uri url);
        string PerformPost(Uri url, Dictionary<string, string> values);
#endif
        void PerformGetAsync(Uri requestUrl, Action<Stream> onSuccess = null, Action<Exception> onError = null, Action onFinally = null);
        void PerformPostAsync(Uri url, Dictionary<string, string> values, Action<string> onSuccess = null, Action<Exception> onError = null, Action onFinally = null);
    }
}
