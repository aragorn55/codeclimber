using System.Web.Mvc;
using Ninject.Core;
using NinjectFilterInjectorTest.Models;

namespace NinjectFilterInjectorTest.ActionFilters
{
    public class TitleActionFilterAttribute : ActionFilterAttribute
    {
        /*
        * If you want to remove the attribute make sure to
        * uncomment the relevant code in ServiceModule class inside 
        * the Global.asax.cs file
        */

        [Inject]
        public IGreetingService _service { get; set; }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var result = filterContext.Result as ViewResult;
            if (result != null)
            {
                result.ViewData["Title"] += " - " + _service.GetGreeting();
            }
        }
    }
}