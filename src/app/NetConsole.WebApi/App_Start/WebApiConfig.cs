using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
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
                name: "GetCommands",
                routeTemplate: "api/commands",
                defaults: new {controller = "commands", action = "Commands" },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) }
                );

            config.Routes.MapHttpRoute(
                name: "MetaApi",
                routeTemplate: "api/commands/meta",
                defaults: new {controller = "commands", action = "Meta" },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Get)}
                );

            config.Routes.MapHttpRoute(
                name: "PerformApi",
                routeTemplate: "api/commands/perform",
                defaults: new { controller = "commands", action = "Perform" },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Post)}
                );

            config.Routes.MapHttpRoute(
                name: "CommandApi",
                routeTemplate: "api/commands/{cmdName}/{action}",
                defaults: new { controller = "commands", action = "Get" },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) }
            );
        }
    }
}
