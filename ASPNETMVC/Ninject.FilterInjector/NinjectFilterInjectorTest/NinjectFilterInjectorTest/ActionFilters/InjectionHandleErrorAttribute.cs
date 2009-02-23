using System;
using System.Web.Mvc;
using Ninject.Core;
using NinjectFilterInjectorTest.Models;

namespace NinjectFilterInjectorTest.ActionFilters
{
    public class InjectionHandleErrorAttribute: HandleErrorAttribute
    {
        [Inject]
        public IErrorHandler _errorModule { get; set; }

        public override void OnException(ExceptionContext filterContext)
        {
            if(_errorModule.ShouldHandleException)
            {
                filterContext.Exception = new ApplicationException(_errorModule.ExceptionMessage);
                base.OnException(filterContext);
            }
        }
    }
}
