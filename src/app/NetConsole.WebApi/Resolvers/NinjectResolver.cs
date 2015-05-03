using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using Ninject;

namespace NetConsole.WebApi.Resolvers
{
    public class NinjectResolver : NinjectScope, IDependencyResolver
    {

        protected IKernel _kernel;

        public NinjectResolver(IKernel kernel)
            : base(kernel)
        {
            if (kernel == null)
                throw new ArgumentNullException("kernel");

            _kernel = kernel;
        }

        public IDependencyScope BeginScope()
        {
            return new NinjectScope(_kernel.BeginBlock());
        }
    }
}