using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using NetConsole.Core.Grammar;

namespace NetConsole.WebApi.Interfaces
{
    public interface IRepository<out T, out TMeta, out TReturn> where TMeta : IMetadata
    {

        IEnumerable<T> GetAll();

        T GetInstance(string name);

        TMeta GetInstanceMetadata(string name);

        IEnumerable<TMeta> GetAllMetadata();

        bool Contains(string name);

        TReturn[] Perform(string action, bool from);

    }
}