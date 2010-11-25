using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CodeClimber.GoogleReaderConnector.Exceptions;
using System.Net;
using System.IO;
using System.Collections.Specialized;

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
                return _auth;
            }
            set { _auth = value; }
        }

        public bool HasAuth()
        {
            return !String.IsNullOrEmpty(_auth);
        }

        public bool Login()
        {
            Uri loginUri = _urlBuilder.GetLoginUri();
            NameValueCollection postData = _urlBuilder.GetLoginData(Username,Password);
            string response;
            // Get the response that needs to be parsed.
            try
            {
                response = _httpService.PerformPost(loginUri, postData);
            }
            catch (IncorrectUsernameOrPasswordException)
            {
                return false;
            }
            // Get auth token.
           _auth = ParseAuthToken(response);
           return true;
        }

        public void LoginAsync(Action onSuccess = null, Action<Exception> onError = null, Action onFinally = null)
        {
            Uri loginUri = _urlBuilder.GetLoginUri();
            NameValueCollection postData = _urlBuilder.GetLoginData(Username, Password);
            // Get the response that needs to be parsed.
            _httpService.PerformPostAsync(loginUri, postData,
                (response) => {
                    _auth = ParseAuthToken(response);
                    if (onSuccess != null)
                    {
                        onSuccess();
                    }
                },
                onError, onFinally);
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
