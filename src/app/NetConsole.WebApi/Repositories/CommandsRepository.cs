using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetConsole.Core.Grammar;
using NetConsole.Core.Interfaces;
using NetConsole.WebApi.Interfaces;
using NetConsole.WebApi.Metadata;

namespace NetConsole.WebApi.Repositories
{
    public class CommandsRepository : IRepository<ICommand, ICommandMetadata<ActionMeta>, ReturnInfo>
    {

        private readonly ICommandManager _manager;
        private readonly ICommandMetadataFactory<ICommandMetadata<ActionMeta>, ActionMeta> _factory;

        public CommandsRepository(ICommandManager manager, ICommandMetadataFactory<ICommandMetadata<ActionMeta>, ActionMeta> factory)
        {
            _manager = manager;
            _factory = factory;
            _factory.RegisterAllMetadata(_manager.Factory.GetAll());
        }

        public IEnumerable<ICommand> GetAll()
        {
            return _manager.Factory.GetAll();
        }

        public ICommand GetInstance(string name)
        {
            return _manager.Factory.GetInstance(name);
        }

        public ICommandMetadata<ActionMeta> GetInstanceMetadata(string name)
        {
            return _factory.GetInstanceMetadata(name);
        }

        public IEnumerable<ICommandMetadata<ActionMeta>> GetAllMetadata()
        {
            return _factory.GetAllMetadata();
        }

        public bool Contains(string name)
        {
            return _manager.Factory.Contains(name);
        }

        public ReturnInfo[] Perform(string action, bool fromFile)
        {
            return fromFile ? _manager.GetOutputFromFile(action) : _manager.GetOutputFromString(action);
        }
    }
}