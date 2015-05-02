using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetConsole.WebApi.Exceptions
{
    public class NullCommandException : NetConsoleWebApiException
    {

        public NullCommandException() : base("Null command instance is not suitable to extract metadata from.")
        {
            
        }

        public NullCommandException(string message) : base(message)
        {
        }

        public NullCommandException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}