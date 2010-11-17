using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace CodeClimber.GoogleReaderConnector
{
    public interface IHttpService: IDisposable
    {
        IClientLoginService ClientLogin { get; set; }
        WebResponse PerformGet(Uri url, bool authenticate);
        WebResponse PerformPost(Uri url, string postData);
    }
}
