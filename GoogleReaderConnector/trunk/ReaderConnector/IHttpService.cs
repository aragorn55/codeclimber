﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace CodeClimber.GoogleReaderConnector
{
    public interface IHttpService: IDisposable
    {
        WebResponse PerformGet(Uri url, object parameters);
    }
}
