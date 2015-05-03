using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetConsole.WebApi.Exceptions
{
    public class DuplicatedCommandMetadataException  : NetConsoleWebApiException
    {
        public DuplicatedCommandMetadataException(string cmdName) : base(string.Format("Metadata associated with {0} command is already registered.", cmdName))
        {
        }

        public DuplicatedCommandMetadataException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}