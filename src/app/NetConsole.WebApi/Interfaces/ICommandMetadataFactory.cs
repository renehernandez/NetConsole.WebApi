using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetConsole.Core.Interfaces;

namespace NetConsole.WebApi.Interfaces
{
    public interface ICommandMetadataFactory<out T, TK> where T : ICommandMetadata<TK>
                                                    where TK : IActionMeta
    {

        void RegisterInstanceMetadata(ICommand command);

        void RegisterAllMetadata(IEnumerable<ICommand> commands);

        T GetInstanceMetadata(string name);

        IEnumerable<T> GetAllMetadata();

        bool Contains(string name);

    }
}