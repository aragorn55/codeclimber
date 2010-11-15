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

        public String Auth { get; set; }

        public HttpService()
        {

        }

        public HttpService(string authorization)
        {
            Auth = authorization;
        }

        #region IHttpService Members

        public System.Net.WebResponse PerformGet(Uri url, object parameters)
        {
            HttpWebRequest request = null;
            request = (HttpWebRequest)WebRequest.Create(url);

            request.Headers.Add("Authorization", "GoogleLogin auth=" + Auth);


            // Get the response, validate and return.
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            if (response == null)
                throw new GoogleResponseNullException();
            else if (response.StatusCode != HttpStatusCode.OK)
                throw new GoogleResponseException(response.StatusCode,
                    response.StatusDescription);

            StreamReader reader = new StreamReader(response.GetResponseStream());
            //string test = reader.ReadToEnd();

            return response;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            
        }

        #endregion
    }
}
