using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;

namespace NetConsole.WebApi.Tests
{
    public class RouteMocker
    {
        private readonly HttpRequestMessage _request;
        private readonly IHttpControllerSelector _controllerSelector;
        private readonly HttpControllerContext _controllerContext;

        public RouteMocker(HttpConfiguration config, HttpRequestMessage request)
        {
            _request = request;

            var routeData = config.Routes.GetRouteData(_request);
            _request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            _request.Properties[HttpPropertyKeys.HttpRouteDataKey] = routeData;
            _controllerSelector = new DefaultHttpControllerSelector(config);
            _controllerContext = new HttpControllerContext(config, routeData, _request);
        }

        public Type GetControllerType()
        {
            var descriptor = _controllerSelector.SelectController(_request);
            _controllerContext.ControllerDescriptor = descriptor;
            return descriptor.ControllerType;
        }

        public string GetActionName()
        {
            if (_controllerContext.ControllerDescriptor == null)
                GetControllerType();

            var actionSelector = new ApiControllerActionSelector();
            var descriptor = actionSelector.SelectAction(_controllerContext);
            return descriptor.ActionName;
        }

        //public string GetMethodName()
        //{
        //    if (_controllerContext.ControllerDescriptor == null)
        //        GetControllerType();

        //    var actionSelector = new ApiControllerActionSelector();
        //    var descriptor = actionSelector.SelectAction(_controllerContext);
        //}

        public void SetUpController(ApiController controller)
        {
            controller.ControllerContext = _controllerContext;
            controller.Request = _request;
        }
    }
}
