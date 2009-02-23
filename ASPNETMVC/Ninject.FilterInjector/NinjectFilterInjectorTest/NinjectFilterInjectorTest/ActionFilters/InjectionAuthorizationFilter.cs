using System.Web.Mvc;
using Ninject.Core;
using NinjectFilterInjectorTest.Models;

namespace NinjectFilterInjectorTest.ActionFilters
{
  public class InjectionAuthorizeFilter: AuthorizeAttribute
  {
    [Inject]
    public IAuthorizationModule _authModule{ get; set; }

    public override void OnAuthorization(AuthorizationContext filterContext)
    {
      if (_authModule == null || !_authModule.IsAuthorized)
        filterContext.Result = new EmptyResult();
    }
  }
}
