
using System.Web.Mvc;
using NinjectFilterInjectorTest.ActionFilters;

namespace NinjectFilterInjectorTest.Controllers
{
    [HandleError]
    [TitleActionFilter]
    public class HomeController : Controller
    {
        [InjectionAuthorizeFilter]
        public ActionResult Index()
        {
            ViewData["Title"] = "Home Page";
            ViewData["Message"] = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
