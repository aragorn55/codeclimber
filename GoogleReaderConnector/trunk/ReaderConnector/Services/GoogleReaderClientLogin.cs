using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CodeClimber.GoogleReaderConnector.Exceptions;
using System.Net;
using System.IO;

namespace CodeClimber.GoogleReaderConnector.Services
{
    public class GoogleReaderClientLogin: IClientLoginService
    {
        private IHttpService _httpService;
        private IUriBuilder _urlBuilder;

        public string Username { get; set; }
        public string Password { get; set; }

        public GoogleReaderClientLogin(string username, string password, IHttpService httpService, IUriBuilder urlBuilder)
        {
            Username = username;
            Password = password;
            _httpService = httpService;
            _urlBuilder = urlBuilder;
        }

        private string _auth;
        public string Auth
        {
            get 
            {
                if (String.IsNullOrEmpty(_auth))
                {
                    _auth = PerformClientLogin(Username, Password);
                }
                return _auth;
            }
            private set { _auth = value; }
        }

        private string PerformClientLogin(string Username, string Password)
        {
            Uri loginUri = _urlBuilder.GetLoginUri();
            string postData = _urlBuilder.GetLoginData(Username,Password);
            // Get the response that needs to be parsed.
            string response = new StreamReader((_httpService.PerformPost(loginUri, postData).GetResponseStream())).ReadToEnd();
            

            // Get auth token.
            string auth = ParseAuthToken(response);
            return auth;
        }


        public void ResetAuth()
        {
            Auth = null;
        }


        private static string ParseAuthToken(string response)
        {
            // Get the auth token.
            string auth = "";
            try
            {
                auth = new Regex(@"Auth=(?<auth>\S+)").Match(response).Result("${auth}");
            }
            catch (Exception ex)
            {
                throw new AuthTokenException(ex.Message);
            }

            // Validate token.
            if (string.IsNullOrEmpty(auth))
            {
                throw new AuthTokenException("Missing or invalid 'Auth' token.");
            }

            // Use this token in the header of each new request.
            return auth;
        }

    }
}
