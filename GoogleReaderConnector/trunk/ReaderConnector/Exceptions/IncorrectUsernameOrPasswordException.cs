using System.Net;

namespace CodeClimber.GoogleReaderConnector.Exceptions
{
    internal class IncorrectUsernameOrPasswordException : LoginFailedException
    {
        /// <summary>
        /// Fill in the exception.
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="statusDescription"></param>
        public IncorrectUsernameOrPasswordException(HttpStatusCode statusCode, string statusDescription)
            : base(statusCode, statusDescription)
        {

        }
    }
}
