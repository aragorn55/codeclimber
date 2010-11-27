using System;

namespace CodeClimber.GoogleReaderConnector.Exceptions
{
    public class NetworkConnectionException : Exception
    {
        /// <summary>
        /// Fill in the exception.
        /// </summary>
        public NetworkConnectionException()
            : base("No or empty response received from Google.")
        {

        }

        public NetworkConnectionException(string message)
            : base(message)
        {

        }
    }
}
