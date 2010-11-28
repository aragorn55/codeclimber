using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using CodeClimber.GoogleReaderConnector.Services;

namespace CodeClimber.GoogleReaderConnector
{
    public class GoogleReaderFactory
    {
        public static ReaderServiceAsync CreateReaderServiceAsync(string username, string password, string clientName, string auth = null)
        {
            IUriBuilder urlBuilder = new GoogleReaderUrlBuilder(clientName);
            IHttpService httpService = new HttpService();
            IClientLoginService loginService = new GoogleReaderClientLogin(username, password, httpService, urlBuilder);
            if (!String.IsNullOrEmpty(auth))
                loginService.Auth = auth;
            httpService.ClientLogin = loginService;

           return new ReaderServiceAsync(urlBuilder, httpService);
        }

#if !WINDOWS_PHONE
        public static ReaderService CreateReaderService(string username, string password, string clientName, string auth = null)
        {
            IUriBuilder urlBuilder = new GoogleReaderUrlBuilder(clientName);
            IHttpService httpService = new HttpService();
            IClientLoginService loginService = new GoogleReaderClientLogin(username, password, httpService, urlBuilder);
            if (!String.IsNullOrEmpty(auth))
                loginService.Auth = auth;
            httpService.ClientLogin = loginService;

            return new ReaderService(urlBuilder, httpService);
        }
#endif
    }
}
