using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeClimber.GoogleReaderConnector
{
    public class ReaderService : IDisposable
    {
        private const string SERVICENAME = "reader";

        public string Username { get; set; }
        public string Password { get; set; }
        public string ClientName { get; set; }

        public IHttpService HttpService { get; set; }

        public ReaderService(string username, string password, string clientName)
        { 

        }

        public void Dispose()
        {
            if (HttpService != null)
                HttpService.Dispose();
        }

    }
}
