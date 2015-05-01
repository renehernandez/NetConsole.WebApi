using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetConsole.Core.Interfaces;
using NetConsole.WebApi.Interfaces;

namespace NetConsole.WebApi.Metadata
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
                throw new Exception("No metadata information for this command");

            return _cache[name];
        }

        public IEnumerable<CommandMetadata> GetAllMetadata()
        {
            return _cache.Values;
        }
    }
}