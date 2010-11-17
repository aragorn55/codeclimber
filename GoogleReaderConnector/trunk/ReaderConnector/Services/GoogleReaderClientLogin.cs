using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeClimber.GoogleReaderConnector.Services
{
    public class GoogleReaderClientLogin: IClientLoginService
    {
        #region IClientLoginService Members

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
            return "abcde";
        }

        public string Username { get; set; }
        public string Password { get; set; }

        public void ResetAuth()
        {
            Auth = null;
        }

        #endregion
    }
}
