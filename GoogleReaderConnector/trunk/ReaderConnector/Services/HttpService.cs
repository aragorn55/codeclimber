using System;
using System.Collections.Generic;
using System.Linq;
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

        public Stream PerformGet(Uri url, bool authenticate)
        {
            WebClient webClient = new WebClient();

            if (authenticate && ClientLogin == null)
                throw new ArgumentNullException("ClientLogin","If you want authentication you have to specify the ClientLogin class to use");

            bool ok = false;
            int retries = 0;
            // Get the response, validate and return.
            Stream responseStream = null;
            while (!ok)
            {
                try
                {
                    if (authenticate)
                        webClient.Headers.Add("Authorization", "GoogleLogin auth=" + ClientLogin.Auth);

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
                        ClientLogin.ResetAuth();
                        if (!authenticate || retries++ > 3)
                            throw new LoginFailedException(actualResponse.StatusCode, actualResponse.StatusDescription);
                        continue;
                    }
                    if (actualResponse.StatusCode != HttpStatusCode.OK)
                        throw new GoogleResponseException(actualResponse.StatusCode,
                                                          actualResponse.StatusDescription);
                }
                ok = true;
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
                if (faultResponse.StatusCode != HttpStatusCode.OK)
                    throw new LoginFailedException(
                        faultResponse.StatusCode, faultResponse.StatusDescription);
            }

            ASCIIEncoding ascii = new ASCIIEncoding();
            return ascii.GetString(response);
        }

        public void PerformGetAsync(Uri requestUrl, bool authenticate, Action<Stream> onGetCompleted = null, Action<Exception> onError = null, Action onFinally = null, int count = 0)
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
                                ClientLogin.ResetAuth();
                                if (!authenticate || count++ > 3)
                                {
                                    if (onError != null)
                                    {
                                        onError(e.Error);
                                    }
                                    return;
                                }
                                PerformGetAsync(requestUrl,authenticate, onGetCompleted, onError, onFinally, count);
                                return;
                            }

                            if (onError != null)
                            {
                                onError(e.Error);
                            }
                            return;
                        }

                        if (onGetCompleted != null)
                        {
                            onGetCompleted(e.Result);
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
