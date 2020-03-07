using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace NDMS.Apis.DependencyResolution
{
    /// <summary>
    /// Implements a Controller activator and hooks Structure map inside
    /// </summary>
    public class StructureMapWebApiControllerActivator : IHttpControllerActivator
    {
        private readonly IContainer _container;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="container">Structure map container reference</param>
        public StructureMapWebApiControllerActivator(IContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Will be invoked when a need to create a new instance of HTTP Controller
        /// </summary>
        /// <param name="request"></param>
        /// <param name="controllerDescriptor"></param>
        /// <param name="controllerType"></param>
        /// <returns></returns>
        public IHttpController Create(HttpRequestMessage request, 
            HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            var nested = _container.GetNestedContainer();
            var instance = nested.GetInstance(controllerType) as IHttpController;
            request.RegisterForDispose(nested);
            return instance;
        }
    }
}