using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeClimber.GoogleReaderConnector
{
    public interface IUriBuilder
    {
        Uri BuildUri(UrlType type, string url);
    }
}
