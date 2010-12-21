using System;
using System.Collections.Generic;
using System.Net;
using CodeClimber.GoogleReaderConnector.Exceptions;
using System.IO;

#if !WINDOWS_PHONE
using System.Web;
#endif

namespace CodeClimber.GoogleReaderConnector.Services
{
    public class HttpService: IHttpService
    {
        public IClientLoginService ClientLogin { get; set; }

        public HttpService()
        { }

        public HttpService(IClientLoginService clientLogin)
        {
            ClientLogin = clientLogin;
        }

        #region IHttpService Members

#if !WINDOWS_PHONE
        public Stream PerformGet(Uri url)
        {
            WebClient webClient = new WebClient();

            // Get the response, validate and return.
            Stream responseStream = null;
            try
            {
                webClient.Headers.Add("Authorization", "GoogleLogin auth=" + ClientLogin.Auth);
                responseStream = webClient.OpenRead(url);
            }
            catch (WebException webex)
            {
                HandleConnectionError(webex);
            }

            return responseStream;
        }

        public string PerformPost(Uri url, Dictionary<string, string> values)
        {
            WebClient webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            string result="";
            try
            {
                webClient.Headers.Add("Authorization", "GoogleLogin auth=" + ClientLogin.Auth);
                result = webClient.UploadString(url, ConverToString(values));
            }
            catch (WebException webex)
            {
                HandleConnectionError(webex,true);
            }
            return result;
        }
#endif

        private static void HandleConnectionError(WebException webex, bool throwWrongUsernameAndPassword=false)
        {
            HttpWebResponse faultResponse = webex.Response as HttpWebResponse;
            if (faultResponse == null)
                throw new NetworkConnectionException(webex.Status + ": " + webex.Message);

            if (throwWrongUsernameAndPassword)
            {
                if (faultResponse.StatusCode == HttpStatusCode.Forbidden)
                    throw new IncorrectUsernameOrPasswordException(faultResponse.StatusCode, faultResponse.StatusDescription);
            }
            else
            {
                  if (faultResponse.StatusCode == HttpStatusCode.Unauthorized)
                      throw new LoginFailedException(faultResponse.StatusCode, faultResponse.StatusDescription);
            }

            if (faultResponse.StatusCode != HttpStatusCode.OK)
                throw new GoogleResponseException(faultResponse.StatusCode, faultResponse.StatusDescription);
        }



        public void PerformPostAsync(Uri url, Dictionary<string, string> values, Action<string> onSuccess = null, Action<Exception> onError = null, Action onFinally = null)
        {
            WebClient webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

            webClient.UploadStringCompleted += delegate(object sender, UploadStringCompletedEventArgs e)
                {
                    try
                    {
                        if (e.Error != null)
                        {
                            if (onError != null)
                            {
                                WebException webex = e.Error as WebException;
                                HttpWebResponse faultResponse = webex.Response as HttpWebResponse;
                                
                                if (faultResponse == null)
                                    onError(new NetworkConnectionException(webex.Status + ": " + webex.Message));

                                else if (faultResponse.StatusCode == HttpStatusCode.Forbidden)
                                    onError(new IncorrectUsernameOrPasswordException(
                                        faultResponse.StatusCode, faultResponse.StatusDescription));

                                else if (faultResponse.StatusCode != HttpStatusCode.OK)
                                    onError(new LoginFailedException(
                                        faultResponse.StatusCode, faultResponse.StatusDescription));
                                else
                                    onError(e.Error);
                            }
                            return;
                        }

                        if (onSuccess != null)
                        {
                            onSuccess(e.Result);
                        }
                    }
                    finally
                    {
                        if (onFinally != null)
                        {
                            onFinally();
                        }
                    } 
                };
            webClient.UploadStringAsync(url, ConverToString(values));
        }

        public void PerformGetAsync(Uri requestUrl, Action<Stream> onSuccess = null, Action<Exception> onError = null, Action onFinally = null)
        {
            WebClient webClient = new WebClient();

            webClient.Headers[HttpRequestHeader.Authorization] = "GoogleLogin auth=" + ClientLogin.Auth;

            webClient.OpenReadCompleted += delegate(object sender, OpenReadCompletedEventArgs e)
                {
                    try
                    {
                        if (e.Error != null)
                        {
                            if (onError != null)
                            {
                                WebException webex = e.Error as WebException;
                                HttpWebResponse actualResponse = webex.Response as HttpWebResponse;

                                if (actualResponse == null)
                                {
                                    onError(new NetworkConnectionException(webex.Status + ": " + webex.Message));
                                }
                                else if (actualResponse.StatusCode == HttpStatusCode.Unauthorized)
                                {
                                    onError(new LoginFailedException(actualResponse.StatusCode, actualResponse.StatusDescription));
                                    return;
                                }
                                else onError(new GoogleResponseException(actualResponse.StatusCode,
                                                              actualResponse.StatusDescription));
                            }
                            return;
                        }

                        if (onSuccess != null)
                        {
                            onSuccess(e.Result);
                        }
                    }
                    finally
                    {
                        if (onFinally != null)
                        {
                            onFinally();
                        }
                    }                          
                };

            webClient.OpenReadAsync(requestUrl);
        }

        #endregion

        private static String ConverToString(Dictionary<string, string> dict)
        {
            string[] vars = new string[dict.Count];
            int i = 0;
            foreach (KeyValuePair<string, string> keyValuePair in dict)
            {
                vars[i++]=String.Format("{0}={1}", keyValuePair.Key, HttpUtility.UrlEncode(keyValuePair.Value));
            }
            return string.Join("&", vars);
        }

    }
}
