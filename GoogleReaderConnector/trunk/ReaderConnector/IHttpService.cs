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
    }
}
