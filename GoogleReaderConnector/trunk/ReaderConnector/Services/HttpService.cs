using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using CodeClimber.GoogleReaderConnector.Exceptions;
using System.IO;

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

        public System.Net.WebResponse PerformGet(Uri url, bool authenticate)
        {
            if (authenticate && ClientLogin == null)
                throw new ArgumentNullException("ClientLogin","If you want authentication you have to specify the ClientLogin class to use");
            HttpWebRequest request = null;
            request = (HttpWebRequest)WebRequest.Create(url);

            if (authenticate)
            {
                request.Headers.Add("Authorization", "GoogleLogin auth=" + ClientLogin.Auth);
            }

            bool ok = false;
            int retries = 0;
            HttpWebResponse response = null;
            // Get the response, validate and return.
            while (!ok)
            {
                try
                {
                    response = request.GetResponse() as HttpWebResponse;
                    if (response == null)
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
                        ok = false;
                        if (retries++ > 3)
                            throw new LoginFailedException(actualResponse.StatusCode, actualResponse.StatusDescription);
                        continue;
                    }
                    else if (actualResponse.StatusCode != HttpStatusCode.OK)
                        throw new GoogleResponseException(actualResponse.StatusCode,
                            actualResponse.StatusDescription);
                }
                ok = true;
            }

            return response;
        }

        public System.Net.WebResponse PerformPost(Uri url, string postData)
        {
            // Get the current post data and encode.
            ASCIIEncoding ascii = new ASCIIEncoding();
            byte[] encodedPostData = ascii.GetBytes(postData);

            // Prepare request.
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = encodedPostData.Length;

            // Write login info to the request.
            using (Stream newStream = request.GetRequestStream())
                newStream.Write(encodedPostData, 0, encodedPostData.Length);

            // Get the response that will contain the Auth token.
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                HttpWebResponse faultResponse = ex.Response as HttpWebResponse;
                if (faultResponse != null && faultResponse.StatusCode == HttpStatusCode.Forbidden)
                    throw new IncorrectUsernameOrPasswordException(
                        faultResponse.StatusCode, faultResponse.StatusDescription);
                else
                    throw;
            }

            // Check for login failed.
            if (response.StatusCode != HttpStatusCode.OK)
                throw new LoginFailedException(
                    response.StatusCode, response.StatusDescription);

            return response;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            ClientLogin = null;
        }

        #endregion
    }
}
