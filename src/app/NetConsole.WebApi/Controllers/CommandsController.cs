using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NetConsole.Core.Interfaces;

namespace NetConsole.WebApi.Controllers
{
    public class CommandsController : ApiController
    {

        private readonly ICommandManager _manager;

        public CommandsController(ICommandManager manager)
        {
            _manager = manager;
        }

        public HttpResponseMessage GetAllCommands()
        {
            var commands = _manager.Factory.GetAll();

            var response = Request.CreateResponse(HttpStatusCode.OK, commands);
            return response;
        }

        public HttpResponseMessage GetCommmand(string cmdName)
        {
            if (!_manager.Factory.Contains(cmdName))
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));

            var command = _manager.Factory.GetInstance(cmdName);
            var response = Request.CreateResponse(HttpStatusCode.OK, command);
            return response;
        }

        public HttpResponseMessage PostAction([FromBody] string commandAction)
        {
            var output = _manager.GetOutputFromString(commandAction);

            var response = Request.CreateResponse(HttpStatusCode.OK, output);
            return response;
        }

    }
}
