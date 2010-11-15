using System;

namespace CodeClimber.GoogleReaderConnector.Exceptions
{
    public class AuthTokenException : Exception
    {
        /// <summary>
        /// Fill in the exception.
        /// </summary>
        /// <param name="message"></param>
        public AuthTokenException(string message)
            : base(message)
        {

        }
    }
}
