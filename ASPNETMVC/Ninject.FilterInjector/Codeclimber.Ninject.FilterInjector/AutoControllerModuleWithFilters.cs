#region Using Directives
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Ninject.Core;
using Ninject.Core.Activation;
using Ninject.Core.Behavior;
using Ninject.Core.Parameters;
using Ninject.Core.Infrastructure;
#endregion

namespace Codeclimber.Ninject.FilterInjector
{
    /// <summary>
    /// A module that automatically loads and registers all MVC controllers in specific assemblies.
    /// </summary>
    public class AutoControllerModuleWithFilters : StandardModule
    {
        /*----------------------------------------------------------------------------------------*/
        #region Fields
        private readonly IEnumerable<Assembly> _assemblies;
        #endregion
        /*----------------------------------------------------------------------------------------*/
        #region Controllers
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoControllerModule"/> class.
        /// </summary>
        /// <param name="assemblies">The assemblies to scan for controllers.</param>
        public AutoControllerModuleWithFilters(IEnumerable<Assembly> assemblies)
        {
            _assemblies = assemblies;
        }
        /*----------------------------------------------------------------------------------------*/
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoControllerModule"/> class.
        /// </summary>
        /// <param name="assemblies">The assemblies to scan for controllers.</param>
        public AutoControllerModuleWithFilters(params Assembly[] assemblies)
        {
            _assemblies = assemblies;
        }
        /*----------------------------------------------------------------------------------------*/
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoControllerModule"/> class.
        /// </summary>
        /// <param name="assemblyNames">The names of assemblies which should be scanned for controllers.</param>
        public AutoControllerModuleWithFilters(IEnumerable<string> assemblyNames)
        {
            _assemblies = assemblyNames.Convert(s => Assembly.Load(s));
        }
        /*----------------------------------------------------------------------------------------*/
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoControllerModule"/> class.
        /// </summary>
        /// <param name="assemblyNames">The names of assemblies which should be scanned for controllers.</param>
        public AutoControllerModuleWithFilters(params string[] assemblyNames)
        {
            _assemblies = assemblyNames.Convert(s => Assembly.Load(s));
        }
        #endregion
        /*----------------------------------------------------------------------------------------*/
        #region Public Methods
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            //Define binding for custom Action Invoker
            Bind<IActionInvoker>().To<NinjectActionInvoker>().Using<SingletonBehavior>();
            _assemblies.Each(RegisterControllers);
        }
        #endregion
        /*----------------------------------------------------------------------------------------*/
        #region Private Methods
        private void RegisterControllers(Assembly assembly)
        {
            foreach (Type type in assembly.GetExportedTypes())
            {
                if (!type.IsPublic || type.IsAbstract || type.IsInterface || type.IsValueType)
                    continue;

                if (!typeof(IController).IsAssignableFrom(type))
                    continue;

                string name = GetNameForController(type);

                Bind(typeof (IController)).To(type).OnlyIf(ctx => CheckControllerName(ctx, name))
                    .InjectPropertiesWhere(p => p.Name == "ActionInvoker");
            }
        }
        /*----------------------------------------------------------------------------------------*/
        private static bool CheckControllerName(IContext context, string name)
        {
            if (context.Parameters == null)
                return false;

            var parameter = context.Parameters.Get<VariableParameter>("controllerName");

            if (parameter == null)
                return false;

            var value = parameter.GetValue(context) as string;

            if (value == null)
                return false;

            return value.Equals(name, StringComparison.CurrentCultureIgnoreCase);
        }
        /*----------------------------------------------------------------------------------------*/
        private static string GetNameForController(Type type)
        {
            string name = type.Name;

            if (name.EndsWith("Controller"))
                return name.Substring(0, name.IndexOf("Controller"));
            else
                return name;
        }
        #endregion
        /*----------------------------------------------------------------------------------------*/
    }
}