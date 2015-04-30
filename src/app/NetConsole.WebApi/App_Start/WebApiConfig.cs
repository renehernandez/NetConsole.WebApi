using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using NetConsole.WebApi.Registrations;
using NetConsole.WebApi.Resolvers;
using Ninject;

namespace NetConsole.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var registrations = new NinjectRegistrations();
            var kernel = new StandardKernel(registrations);
            var ninjectResolver = new NinjectResolver(kernel);
            config.DependencyResolver = ninjectResolver;

            config.Routes.MapHttpRoute(
                name: "Commands",
                routeTemplate: "api/commands/{cmdName}",
                defaults: new { controller = "commands", cmdName = RouteParameter.Optional }
            );
        }
    }
}
