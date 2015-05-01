using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using NetConsole.Core.Interfaces;

namespace NetConsole.WebApi.Extensions
{
    public static class WebApiCommandExtensions
    {

        public static IEnumerable<KeyValuePair<MethodInfo, ParameterInfo[]>> GetActions(this ICommand command)
        {
            return command.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).
                Where(info => !info.IsSpecialName).
                Select(info => new KeyValuePair<MethodInfo, ParameterInfo[]>(info, info.GetParameters()));
        }

    }
}