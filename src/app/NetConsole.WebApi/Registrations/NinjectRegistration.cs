using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetConsole.Core.Factories;
using NetConsole.Core.Grammar;
using NetConsole.Core.Interfaces;
using NetConsole.Core.Managers;
using NetConsole.WebApi.Factories;
using NetConsole.WebApi.Interfaces;
using NetConsole.WebApi.Metadata;
using NetConsole.WebApi.Repositories;
using Ninject.Modules;

namespace NetConsole.WebApi.Registrations
{
    public class NinjectRegistrations : NinjectModule
    {
        public override void Load()
        {
            Bind<ICommandMetadata<ActionMeta>>().To<CommandMetadata>();
            Bind<ICommandFactory>().To<CommandFactory>();
            Bind<ICommandMetadataFactory<ICommandMetadata<ActionMeta>, ActionMeta>>().To<CommandMetadataFactory>();
            Bind<ICommandManager>().To<CommandManager>();
            Bind<IRepository<ICommand, ICommandMetadata<ActionMeta>, ReturnInfo>>().To<CommandsRepository>();
        }
    }
}