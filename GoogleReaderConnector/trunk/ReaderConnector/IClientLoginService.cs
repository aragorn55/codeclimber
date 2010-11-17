using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeClimber.GoogleReaderConnector
{
    public interface IClientLoginService
    {
        string Auth { get; }

        string Username { get; set; }
        string Password { get; set; }

        void ResetAuth();
    }
}
