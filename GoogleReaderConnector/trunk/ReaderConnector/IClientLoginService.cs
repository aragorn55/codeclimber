using System;

namespace CodeClimber.GoogleReaderConnector
{
    public interface IClientLoginService
    {
        string Auth { get; set; }

        string Username { get; set; }
        string Password { get; set; }

        Boolean HasAuth();
        #if !WINDOWS_PHONE
        Boolean Login();
        #endif
        void LoginAsync(Action<bool> onSuccess = null, Action<Exception> onError = null, Action onFinally = null);
    }
}
