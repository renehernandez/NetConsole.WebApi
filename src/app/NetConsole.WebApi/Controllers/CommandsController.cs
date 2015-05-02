using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NetConsole.Core.Grammar;
using NetConsole.Core.Interfaces;
using NetConsole.WebApi.Interfaces;
using NetConsole.WebApi.Metadata;

namespace NetConsole.WebApi.Controllers
{
    public class CommandsController : ApiController
    {

        private readonly IRepository<ICommand, ICommandMetadata<ActionMeta>, ReturnInfo> _repository;

        public CommandsController(IRepository<ICommand, ICommandMetadata<ActionMeta>, ReturnInfo> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [ActionName("Rest")]
        public HttpResponseMessage Get()
        {
            var commands = _repository.GetAll();

            var response = Request.CreateResponse(HttpStatusCode.OK, commands);
            return response;
        }

        [HttpGet]
        [ActionName("Rest")]
        public HttpResponseMessage Get(string cmdName)
        {
            if (!_repository.Contains(cmdName))
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));

            var command = _repository.GetInstance(cmdName);
            var response = Request.CreateResponse(HttpStatusCode.OK, command);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage Meta()
        {
            var meta = _repository.GetAllMetadata();

            var response = Request.CreateResponse(HttpStatusCode.OK, meta);
            return response;
        }

        [HttpGet]
        public HttpResponseMessage Meta(string cmdName)
        {
            if(!_repository.Contains(cmdName))
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));

            var meta = _repository.GetInstanceMetadata(cmdName);

            var response = Request.CreateResponse(HttpStatusCode.OK, meta);
            return response;
        }

        [HttpPost]
        public HttpResponseMessage Perform([FromBody] string commandAction)
        {
            var output = _repository.Perform(commandAction, false);

            var status = output.Any() && output.Last().Status == 0 ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
 
            var response = Request.CreateResponse(status, output);
            return response;
        }

    }
}
