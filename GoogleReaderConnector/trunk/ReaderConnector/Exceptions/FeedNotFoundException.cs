using System;

namespace CodeClimber.GoogleReaderConnector.Exceptions
{
    public class FeedNotFoundException : Exception
    {
        /// <summary>
        /// Fill in the exception.
        /// </summary>
        /// <param name="message"></param>
        public FeedNotFoundException(string message)
            : base(message)
        {

        }
    }
}
