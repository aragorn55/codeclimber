namespace NinjectFilterInjectorTest.Models
{
  public class AuthorizationModuleImpl
    : IAuthorizationModule
  {
    public bool IsAuthorized
    {
      get { return true; }
    }
  }
  public interface IAuthorizationModule
  {
    bool IsAuthorized { get; }
  }
}
