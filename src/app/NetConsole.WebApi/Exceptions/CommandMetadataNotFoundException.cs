using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetConsole.WebApi.Exceptions
{
    public class CommandMetadataNotFoundException : NetConsoleWebApiException
    {
        public CommandMetadataNotFoundException(string cmdName) : base(string.Format("No metadata information for {0} command.", cmdName))
        {
        }

        public CommandMetadataNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}