using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeClimber.GoogleReaderConnector
{
    public interface IClientLoginService
    {
        string Auth { get; set; }

        string Username { get; set; }
        string Password { get; set; }

        Boolean HasAuth();
        Boolean Login();
        void LoginAsync(Action onSuccess = null, Action<Exception> onError = null, Action onFinally = null);
    }
}
