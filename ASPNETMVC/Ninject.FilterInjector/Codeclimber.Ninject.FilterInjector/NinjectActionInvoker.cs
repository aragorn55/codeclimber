using System.Collections.Generic;
using System.Web.Mvc;
using Ninject.Core;

namespace Codeclimber.Ninject.FilterInjector
{
    public class NinjectActionInvoker : ControllerActionInvoker
    {
        private readonly IKernel _kernel;

        public NinjectActionInvoker(IKernel kernel)
        {
            _kernel = kernel;
        }

        protected override ActionExecutedContext InvokeActionMethodWithFilters(
            ControllerContext controllerContext,
            IList<IActionFilter> filters,
            ActionDescriptor actionDescriptor,
            IDictionary<string, object> parameters)
        {
            foreach (IActionFilter actionFilter in filters)
            {
                _kernel.Inject(actionFilter);
            }

            return base.InvokeActionMethodWithFilters(
                controllerContext, filters, actionDescriptor, parameters);
        }

        protected override AuthorizationContext InvokeAuthorizationFilters(
            ControllerContext controllerContext,
            IList<IAuthorizationFilter> filters,
            ActionDescriptor actionDescriptor)
        {
            foreach (IAuthorizationFilter actionFilter in filters)
            {
                _kernel.Inject(actionFilter);
            }

            return base.InvokeAuthorizationFilters(
                controllerContext, filters, actionDescriptor);
        }

        protected override ResultExecutedContext InvokeActionResultWithFilters(
            ControllerContext controllerContext,
            IList<IResultFilter> filters,
            ActionResult actionResult)
        {
            foreach (IActionFilter actionFilter in filters)
            {
                _kernel.Inject(actionFilter);
            }
            return base.InvokeActionResultWithFilters(controllerContext, filters, actionResult);
        }

        protected override ExceptionContext InvokeExceptionFilters(
            ControllerContext controllerContext,
            IList<IExceptionFilter> filters,
            System.Exception exception)
        {
            foreach (IExceptionFilter actionFilter in filters)
            {
                _kernel.Inject(actionFilter);
            }
            return base.InvokeExceptionFilters(controllerContext, filters, exception);
        }
    }
}
