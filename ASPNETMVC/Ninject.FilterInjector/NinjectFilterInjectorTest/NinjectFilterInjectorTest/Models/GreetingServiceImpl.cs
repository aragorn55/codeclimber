namespace NinjectFilterInjectorTest.Models
{
    public class GreetingServiceImpl : IGreetingService
    {
        #region IGreetingService Members

        public string GetGreeting()
        {
            return "Hello from the Action Filter";
        }

        #endregion
    }

    public interface IGreetingService
    {
        string GetGreeting();
    }
}