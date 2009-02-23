
using System;
using System.Web.Mvc;
using NinjectFilterInjectorTest.ActionFilters;

namespace NinjectFilterInjectorTest.Controllers
{
    [InjectionHandleError]
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
            throw new Exception();
            return View();
        }
    }
}
