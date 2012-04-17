using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;
using System.Reflection;
using System.Web.Mvc;

using WebUI.Unity;

namespace WebUI
{
    public class UnityControllerFactory : DefaultControllerFactory
    {
        private IUnityContainer container = new UnityContainer();
        
        public UnityControllerFactory()
        {
            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            section.Configure(container);
          
            var _controllerTypes = from t in Assembly.GetExecutingAssembly().GetTypes()
                                   where typeof(IController).IsAssignableFrom(t)
                                   select t;
        
            foreach (Type t in _controllerTypes)
            {
                container.RegisterType(typeof(IController), t, new TransientLifetimeManager());
            }
        }

        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            return container.Resolve(controllerType) as IController;
        }
    }
}