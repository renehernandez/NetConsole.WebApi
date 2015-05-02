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
        private ICommandManager _stubManager;
        private ICommandMetadataFactory<CommandMetadata, ActionMeta> _stubMetadataFactory;
        private CommandsRepository _repository;
        private HttpConfiguration _config;
        private RouteMocker _routeMocker;
        private HttpRequestMessage _request;

        [SetUp]
        public void SetUp()
        {
            // Stubs
            _stubCommandFactory = MockRepository.GenerateStub<ICommandFactory>();
            _stubCommandFactory.Stub(x => x.Contains("echo")).Return(true);
            _stubCommandFactory.Stub(x => x.GetInstance("echo")).Return(new EchoCommand());
            _stubCommandFactory.Stub(x => x.GetAll())
                .Return(new List<ICommand> {new EchoCommand(), new PromptCommand()});

            _stubMetadataFactory = MockRepository.GenerateStub<ICommandMetadataFactory<CommandMetadata, ActionMeta>>();
            _stubMetadataFactory.Stub(x => x.RegisterInstanceMetadata(new EchoCommand())).IgnoreArguments();
            _stubMetadataFactory.Stub(x => x.RegisterAllMetadata(null)).IgnoreArguments();

            _stubManager = MockRepository.GenerateStub<ICommandManager>();
            _stubManager.Stub(x => x.Factory).Return(_stubCommandFactory);
            _stubManager.Stub(x => x.GetOutputFromString("echo Hello my dear")).Return(new []{ new ReturnInfo("Hello my dear", 0)});
            _repository = new CommandsRepository(_stubManager, _stubMetadataFactory);
            _controller = new CommandsController(_repository);

            // Http Configuration
            _config = new HttpConfiguration();
            _config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
            
            WebApiConfig.Register(_config);
        }

        # region Actions Tests

        [Test]
        public void Test_GetThrowsHttpResponseException()
        {
            //Assert
            Assert.Throws<HttpResponseException>(() => _controller.Get("hello"));
        }

        [Test]
        public void Test_PostActionStatusCode()
        {
            // Arrange
            _routeMocker = new RouteMocker(_config, new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/commands/echo/perform"));
            _routeMocker.SetUpController(_controller);

            // Act
            var response = _controller.Perform("echo:echoed Hello my dear");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public void Test_PostActionContent()
        {
            // Arrange
            _routeMocker = new RouteMocker(_config, new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/commands/echo/perform"));
            _routeMocker.SetUpController(_controller);

            //Act
            var response = _controller.Perform("echo Hello my dear");
            var content = response.Content as ObjectContent;
            var output = content.Value as ReturnInfo[];

            // Assert
            Assert.AreEqual(1, output.Length);
            Assert.AreEqual("Hello my dear", output[0].Output);
        }

        # endregion

        # region Endpoints Tests

        [Test]
        public void Test_MatchMetaUrl()
        {
            // Arrange
            _request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/commands/meta");
            _routeMocker = new RouteMocker(_config, _request);

            // Assert
            Assert.AreEqual(typeof(CommandsController), _routeMocker.GetControllerType());
            Assert.AreEqual(ReflectionHelper.GetMethodName((CommandsController c) => c.Meta()), 
                _routeMocker.GetActionName());
        }

        [Test]
        public void Test_MatchPerformUrl()
        {
            // Arrange
            _request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/commands/perform");
            _routeMocker = new RouteMocker(_config, _request);

            // Assert
            Assert.AreEqual(typeof(CommandsController), _routeMocker.GetControllerType());
            Assert.AreEqual(ReflectionHelper.GetMethodName((CommandsController c) => c.Perform("")),
                _routeMocker.GetActionName());
        }

        [Test]
        public void Test_MatchCommandMetaUrl()
        {
            // Arrange
            _request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/commands/prompt/meta");
            _routeMocker = new RouteMocker(_config, _request);

            // Assert
            Assert.AreEqual(typeof(CommandsController), _routeMocker.GetControllerType());
            Assert.AreEqual(ReflectionHelper.GetMethodName((CommandsController c) => c.Meta("prompt")),
                _routeMocker.GetActionName());
        }

        [Test]
        public void Test_MatchGetUrl()
        {
            // Arrange
            _request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/commands");
            _routeMocker = new RouteMocker(_config, _request);

            // Assert
            Assert.AreEqual(typeof(CommandsController), _routeMocker.GetControllerType());
            Assert.AreEqual("Rest", _routeMocker.GetActionName());
        }

        [Test]
        public void Test_MatchMetaCommandUrl()
        {
            // Arrange
            _request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/commands/prompt");
            _routeMocker = new RouteMocker(_config, _request);

            // Assert
            Assert.AreEqual(typeof(CommandsController), _routeMocker.GetControllerType());
            Assert.AreEqual("Rest", _routeMocker.GetActionName());
        }

        # endregion

    }
}
