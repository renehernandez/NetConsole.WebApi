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
        private IRepository<ICommand, ICommandMetadata<ActionMeta>, ReturnInfo> _repository;
        private HttpConfiguration _config;
        private RouteMocker _routeMocker;
        private HttpRequestMessage _request;

        [SetUp]
        public void SetUp()
        {
            // Stubs
            _repository = MockRepository.GenerateStub<IRepository<ICommand, ICommandMetadata<ActionMeta>, ReturnInfo>>();

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
        public void Test_MetaThrowsHttpResponseException()
        {
            // Assert
            Assert.Throws<HttpResponseException>(() => _controller.Meta("flickr"));
        }

        [Test]
        public void Test_PostActionStatusCodeOk()
        {
            // Arrange
            string input = "prompt";
            _repository.Stub(x => x.Perform(input, false)).Return(new[] { new ReturnInfo("$", 0) });
            _routeMocker = new RouteMocker(_config, new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/commands/perform"));
            _routeMocker.SetUpController(_controller);

            // Act
            var response = _controller.Perform(input);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public void Test_PostActionStatusCodeServerError()
        {
            // Arrange
            string input = "prompt:what error";
            _repository.Stub(x => x.Perform(input, false)).Return(new[] { new ReturnInfo("", 1) });
            _routeMocker = new RouteMocker(_config, new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/commands/perform"));
            _routeMocker.SetUpController(_controller);

            // Act
            var response = _controller.Perform(input);

            // Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Test]
        public void Test_PostActionContent()
        {
            // Arrange
            string input = "echo Hello my dear";
            _repository.Stub(x => x.Perform(input, false)).Return(new[] { new ReturnInfo("Hello my dear", 0) });
            _routeMocker = new RouteMocker(_config, new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/commands/perform"));
            _routeMocker.SetUpController(_controller);

            //Act
            var response = _controller.Perform(input);
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
            Assert.AreEqual("Commands", _routeMocker.GetActionName());
        }

        [Test]
        public void Test_MatchMetaCommandUrl()
        {
            // Arrange
            _request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/commands/prompt");
            _routeMocker = new RouteMocker(_config, _request);

            // Assert
            Assert.AreEqual(typeof(CommandsController), _routeMocker.GetControllerType());
            Assert.AreEqual("Get", _routeMocker.GetActionName());
        }

        # endregion

    }
}
