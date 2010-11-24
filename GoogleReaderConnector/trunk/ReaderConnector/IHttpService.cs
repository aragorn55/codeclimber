using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Collections.Specialized;

namespace CodeClimber.GoogleReaderConnector
{
    public interface IHttpService
    {
        IClientLoginService ClientLogin { get; set; }
        Stream PerformGet(Uri url, bool authenticate);
        string PerformPost(Uri url, NameValueCollection values);

        void PerformGetAsync(Uri requestUrl, bool authenticate, Action<Stream> onSuccess = null, Action<Exception> onError = null, Action onFinally = null, int count = 0);
        void PerformPostAsync(Uri url, NameValueCollection values, Action<string> onSuccess = null, Action<Exception> onError = null, Action onFinally = null);
    }
}
