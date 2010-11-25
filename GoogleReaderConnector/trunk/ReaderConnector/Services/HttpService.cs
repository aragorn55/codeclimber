using System;
using System.Text;
using System.Net;
using CodeClimber.GoogleReaderConnector.Exceptions;
using System.IO;
using System.Collections.Specialized;

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

        public Stream PerformGet(Uri url, bool authenticate, bool retry = true)
        {
            WebClient webClient = new WebClient();

            if (authenticate && ClientLogin == null)
                throw new ArgumentNullException("ClientLogin","If you want authentication you have to specify the ClientLogin class to use");

            // Get the response, validate and return.
            Stream responseStream = null;
            try
            {
                if (authenticate)
                {
                    if (!ClientLogin.HasAuth())
                        ClientLogin.Login();
                    webClient.Headers.Add("Authorization", "GoogleLogin auth=" + ClientLogin.Auth);
                }

                responseStream = webClient.OpenRead(url);
                if (responseStream == null)
                    throw new GoogleResponseNullException();
            }
            catch (WebException webex)
            {
                HttpWebResponse actualResponse = webex.Response as HttpWebResponse;
                if(actualResponse==null)
                    throw new GoogleResponseNullException();
                if (actualResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    if (retry)
                    {
                        if(ClientLogin.Login())
                            return PerformGet(url, authenticate, false);
                    }
                    if (authenticate)
                        throw new LoginFailedException(actualResponse.StatusCode, actualResponse.StatusDescription);
                }
                if (actualResponse.StatusCode != HttpStatusCode.OK)
                    throw new GoogleResponseException(actualResponse.StatusCode,
                                                        actualResponse.StatusDescription);
            }


            return responseStream;
        }

        public string PerformPost(Uri url, NameValueCollection values)
        {
            WebClient webClient = new WebClient();

            // Get the response that will contain the Auth token.
            byte[] response = null;
            try
            {
                response = webClient.UploadValues(url, values);
            }
            catch (WebException ex)
            {
                HttpWebResponse faultResponse = ex.Response as HttpWebResponse;
                if (faultResponse != null && faultResponse.StatusCode == HttpStatusCode.Forbidden)
                    throw new IncorrectUsernameOrPasswordException(
                        faultResponse.StatusCode, faultResponse.StatusDescription);
                if (faultResponse != null && faultResponse.StatusCode != HttpStatusCode.OK)
                    throw new LoginFailedException(
                        faultResponse.StatusCode, faultResponse.StatusDescription);
                if (faultResponse == null)
                    throw new LoginFailedException(0, ex.Status.ToString());
            }

            ASCIIEncoding ascii = new ASCIIEncoding();
            return ascii.GetString(response);
        }

        public void PerformPostAsync(Uri url, NameValueCollection values, Action<string> onSuccess = null, Action<Exception> onError = null, Action onFinally = null)
        {
            WebClient webClient = new WebClient();

            webClient.UploadValuesCompleted += delegate(object sender, UploadValuesCompletedEventArgs e)
                {
                    try
                    {
                        if (e.Error != null)
                        {
                            if (onError != null)
                            {
                                WebException webex = e.Error as WebException;
                                HttpWebResponse faultResponse = webex.Response as HttpWebResponse;
                                if (faultResponse != null && faultResponse.StatusCode == HttpStatusCode.Forbidden)
                                    onError(new IncorrectUsernameOrPasswordException(
                                        faultResponse.StatusCode, faultResponse.StatusDescription));
                                if (faultResponse.StatusCode != HttpStatusCode.OK)
                                    onError(new LoginFailedException(
                                        faultResponse.StatusCode, faultResponse.StatusDescription));
                                else
                                    onError(e.Error);
                            }
                            return;
                        }

                        if (onSuccess != null)
                        {
                            ASCIIEncoding ascii = new ASCIIEncoding();
                            onSuccess(ascii.GetString(e.Result));
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
            webClient.UploadValuesAsync(url,values);
        }

        public void PerformGetAsync(Uri requestUrl, bool authenticate, Action<Stream> onSuccess = null, Action<Exception> onError = null, Action onFinally = null, int count = 0)
        {
            WebClient webClient = new WebClient();

            if (authenticate && ClientLogin == null)
                throw new ArgumentNullException("ClientLogin", "If you want authentication you have to specify the ClientLogin class to use");

            if (authenticate)
                webClient.Headers.Add("Authorization", "GoogleLogin auth=" + ClientLogin.Auth);

            webClient.OpenReadCompleted += delegate(object sender, OpenReadCompletedEventArgs e)
                {
                    try
                    {
                        if (e.Error != null)
                        {
                            WebException webex = e.Error as WebException;
                            HttpWebResponse actualResponse = webex.Response as HttpWebResponse;
                            if (actualResponse.StatusCode == HttpStatusCode.Unauthorized)
                            {
                                if (!authenticate || count++ > 3)
                                {
                                    if (onError != null)
                                    {
                                        onError(new LoginFailedException(actualResponse.StatusCode, actualResponse.StatusDescription));
                                    }
                                    return;
                                }
                                PerformGetAsync(requestUrl, authenticate, onSuccess, onError, onFinally, count);
                                return;
                            }

                            if (onError != null)
                            {
                                onError(new GoogleResponseException(actualResponse.StatusCode,
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

    }
}
