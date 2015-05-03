using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using NetConsole.Core.Attributes;
using NetConsole.Core.Interfaces;
using NetConsole.WebApi.Extensions;
using NetConsole.WebApi.Interfaces;

namespace NetConsole.WebApi.Metadata
{
    public class CommandMetadata : ICommandMetadata<ActionMeta>
    {

        public int ActionsCount { get; set; }

        public ActionMeta[] ActionsMeta { get; set; }
        
        public string Name { get; set; }
        
        public string Overview { get; set; }

        public CommandMetadata(ICommand cmd)
        {
            Name = cmd.Name;
            Overview = cmd.Overview;
            ActionsMeta = cmd.GetActions().Select(kv => new ActionMeta()
            {
                Name = kv.Key.Name,
                ReturnType = kv.Key.ReturnType.Name,
                Default = kv.Key.GetCustomAttributes(typeof(DefaultActionAttribute), true).Length != 0,
                ParamsType = kv.Value.Select(p => p.ParameterType.Name).ToArray()
            }).ToArray();
            ActionsCount = ActionsMeta.Length;
        }

    }
}