using System;
using System.Collections.Generic;
using NetConsole.Core.Interfaces;
using NetConsole.WebApi.Exceptions;
using NetConsole.WebApi.Interfaces;
using NetConsole.WebApi.Metadata;

namespace NetConsole.WebApi.Factories
{
    public class CommandMetadataFactory : ICommandMetadataFactory<CommandMetadata, ActionMeta>
    {

        private static Dictionary<string, CommandMetadata> _cache;


        public CommandMetadataFactory()
        {
            _cache = new Dictionary<string, CommandMetadata>();
        }

        public bool Contains(string name)
        {
            return _cache.ContainsKey(name);
        }

        public void RegisterInstanceMetadata(ICommand command)
        {
            if(command == null)
                throw new NullCommandException();
            if(Contains(command.Name))
                throw new DuplicatedCommandMetadataException(command.Name);

            _cache.Add(command.Name, new CommandMetadata(command));
        }

        public void RegisterAllMetadata(IEnumerable<ICommand> commands)
        {
            foreach (var command in commands)
                RegisterInstanceMetadata(command);
        }

        public CommandMetadata GetInstanceMetadata(string name)
        {
            if(!Contains(name))
                throw new CommandMetadataNotFoundException(name);

            return _cache[name];
        }

        public IEnumerable<CommandMetadata> GetAllMetadata()
        {
            return _cache.Values;
        }
    }
}