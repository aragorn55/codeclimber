using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Codeclimber.Ninject.FilterInjector;
using Ninject.Core;
using Ninject.Framework.Mvc;
using NinjectFilterInjectorTest.Models;

namespace NinjectFilterInjectorTest
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : NinjectHttpApplication
    {
        protected override void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = "" } // Parameter defaults
                );
        }

        protected override IKernel CreateKernel()
        {
            var modules = new IModule[]
                              {
                                  new AutoControllerModuleWithFilters(Assembly.GetExecutingAssembly()),
                                  new ServiceModule(),
                              };
            return new StandardKernel(modules);
        }
    }

    internal class ServiceModule : StandardModule
    {
        public override void Load()
        {
            Bind<IGreetingService>().To<GreetingServiceImpl>();
          Bind<IAuthorizationModule>().To<AuthorizationModuleImpl>();
            /*
             * Uncomment this if you want to remove the [Inject]
             * attribute in the TitleActionFilterAttribute class
             */

            //Bind<TitleActionFilterAttribute>().ToSelf()
            //    .InjectPropertiesWhere(p => p.Name == "Service");
        }
    }
}
