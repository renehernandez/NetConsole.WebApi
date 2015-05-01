using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetConsole.WebApi.Interfaces;

namespace NetConsole.WebApi.Metadata
{
    public class ActionMeta : IActionMeta
    {

        public string Name { get; set; }

        public bool Default { get; set; }

        public string ReturnType { get; set; }
        
        public string[] ParamsType { get; set; }

    }
}