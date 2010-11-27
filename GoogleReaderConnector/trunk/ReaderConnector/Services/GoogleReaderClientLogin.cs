using System;
using System.Text.RegularExpressions;
using CodeClimber.GoogleReaderConnector.Exceptions;
using System.Collections.Specialized;

namespace CodeClimber.GoogleReaderConnector.Services
{
    public class GoogleReaderClientLogin: IClientLoginService
    {
        private readonly IHttpService _httpService;
        private readonly IUriBuilder _urlBuilder;

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
            // Get the response that needs to be parsed.
            try
            {
                string response = _httpService.PerformPost(loginUri, postData);
                // Get auth token.
                _auth = ParseAuthToken(response);
            }
            catch (IncorrectUsernameOrPasswordException)
            {
                return false;
            }
           return true;
        }

        public void LoginAsync(Action<bool> onSuccess = null, Action<Exception> onError = null, Action onFinally = null)
        {
            Uri loginUri = _urlBuilder.GetLoginUri();
            NameValueCollection postData = _urlBuilder.GetLoginData(Username, Password);
            // Get the response that needs to be parsed.
            _httpService.PerformPostAsync(loginUri, postData,
                response => {
                            try
                            {
                                _auth = ParseAuthToken(response);
                            }
                            catch (AuthTokenException ex)
                            {
                                HandleLoginFailure(onError, ex, onSuccess);
                                return;
                            }
                    
                            if (onSuccess != null)
                            {
                                onSuccess(true);
                            }
                },
                ex=> HandleLoginFailure(onError, ex, onSuccess), onFinally);
        }

        private static void HandleLoginFailure(Action<Exception> onError, Exception ex, Action<bool> onSuccess)
        {
            if(onError!=null)
            {
                if (ex is IncorrectUsernameOrPasswordException)
                    onSuccess(false);
                else
                    onError(ex);
            }
        }


        private static string ParseAuthToken(string response)
        {
            // Get the auth token.
            string auth;
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
                throw new AuthTokenException("Missing or invalid 'Auth' token.");

            // Use this token in the header of each new request.
            return auth;
        }

    }
}
