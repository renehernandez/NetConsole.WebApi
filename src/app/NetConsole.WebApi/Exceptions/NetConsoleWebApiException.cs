using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetConsole.WebApi.Exceptions
{
    public class NetConsoleWebApiException : Exception
    {

        public NetConsoleWebApiException(string message) : base(message)
        {
            
        }

        public NetConsoleWebApiException(string message, Exception innerException) : base(message, innerException)
        {
            
        }

    }
}