namespace NinjectFilterInjectorTest.Models
{
    public class ErrorHandlerImpl : IErrorHandler
    {
        public bool ShouldHandleException
        {
            get { return true; }
        }

        public string ExceptionMessage
        {
            get { return "Exception has been replaced by Injected ErrorHandler"; }
        }
    }
}