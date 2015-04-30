using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using Ninject.Parameters;
using Ninject.Syntax;

namespace NetConsole.WebApi.Resolvers
{
    public class NinjectScope : IDependencyScope
    {

        protected IResolutionRoot _resolutionRoot;

        public NinjectScope(IResolutionRoot kernel)
        {
            _resolutionRoot = kernel;
        }

        public void Dispose()
        {
            var disposable = (IDisposable)_resolutionRoot;

            if (disposable != null)
                disposable.Dispose();

            _resolutionRoot = null;
        }

        public object GetService(Type serviceType)
        {
            var request = _resolutionRoot.CreateRequest(serviceType, null, new Parameter[0], true, true);
            return _resolutionRoot.Resolve(request).SingleOrDefault();
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            var request = _resolutionRoot.CreateRequest(serviceType, null, new Parameter[0], true, true);
            return _resolutionRoot.Resolve(request).ToList();
        }
    }
}