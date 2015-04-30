using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetConsole.Core.Interfaces;
using NetConsole.Core.Managers;
using Ninject.Modules;

namespace NetConsole.WebApi.Registrations
{
    public class NinjectRegistrations : NinjectModule
    {
        public override void Load()
        {
            this.Bind<ICommandManager>().To<CommandManager>();
        }
    }
}