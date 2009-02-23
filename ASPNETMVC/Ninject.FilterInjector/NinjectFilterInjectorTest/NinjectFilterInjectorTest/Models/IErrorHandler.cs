namespace NinjectFilterInjectorTest.Models
{
    public interface IErrorHandler
    {
        bool ShouldHandleException { get; }
        string ExceptionMessage { get; }
    }
}