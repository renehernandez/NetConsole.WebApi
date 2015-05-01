using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using NetConsole.Core.Commands;
using NetConsole.Core.Grammar;
using NetConsole.Core.Interfaces;
using NetConsole.Core.Managers;
using NetConsole.WebApi.Controllers;
using NetConsole.WebApi.Interfaces;
using NetConsole.WebApi.Metadata;
using NetConsole.WebApi.Repositories;
using NUnit.Framework;
using Rhino.Mocks;

namespace NetConsole.WebApi.Tests
{
    [TestFixture]
    public class CommandsControllerTest
    {

        private CommandsController _controller;
        private ICommandFactory _stubCommandFactory;
        private CommandManager _stubManager;
        private  ICommandMetadataFactory<CommandMetadata, ActionMeta> _stubMetadataFactory;
        private CommandsRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _stubCommandFactory = MockRepository.GenerateStub<ICommandFactory>();
            _stubCommandFactory.Stub(x => x.Contains("echo")).Return(true);
            _stubCommandFactory.Stub(x => x.GetInstance("echo")).Return(new EchoCommand());
            _stubCommandFactory.Stub(x => x.GetAll())
                .Return(new List<ICommand> {new EchoCommand(), new PromptCommand()});

            _stubMetadataFactory = MockRepository.GenerateStub<ICommandMetadataFactory<CommandMetadata, ActionMeta>>();
            _stubMetadataFactory.Stub(x => x.RegisterInstanceMetadata(new EchoCommand())).IgnoreArguments();
            _stubMetadataFactory.Stub(x => x.RegisterAllMetadata(null)).IgnoreArguments();

            _stubManager = MockRepository.GenerateMock<CommandManager>(_stubCommandFactory);            
            _repository = new CommandsRepository(_stubManager, _stubMetadataFactory);
            _controller = new CommandsController(_repository);
        }

        [Test]
        public void Test_GetCommandThrowsHttpResponseException()
        {
            //Assert
            Assert.Throws<HttpResponseException>(() => _controller.Get("hello"));
        }

        [Test]
        public void Test_PostActionStatusCode()
        {
            SetupControllerForTests(_controller);
            var response = _controller.Perform("echo:echoed Hello my dear");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public void Test_PostActionContent()
        {
            SetupControllerForTests(_controller);
            var response = _controller.Perform("echo:echoed Hello my dear");

            var content = response.Content as ObjectContent;
            var output = content.Value as ReturnInfo[];

            Assert.AreEqual(1, output.Length);
            Assert.AreEqual("Hello my dear", output[0].Output);
        }

        private static void SetupControllerForTests(ApiController controller)
        {
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/commands");
            var route = config.Routes.MapHttpRoute(
                name: "Commands",
                routeTemplate: "api/commands/{cmdName}",
                defaults: new { controller = "commands", cmdName = RouteParameter.Optional }
                );

            var routeData = new HttpRouteData(route);

            controller.ControllerContext = new HttpControllerContext(config, routeData, request);
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
        }

    }
}
