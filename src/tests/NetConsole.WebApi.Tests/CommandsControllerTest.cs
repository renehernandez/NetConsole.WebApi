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
using NUnit.Framework;
using Rhino.Mocks;

namespace NetConsole.WebApi.Tests
{
    [TestFixture]
    public class CommandsControllerTest
    {

        private CommandsController _controller;
        private ICommandFactory _stubFactory;
        private CommandManager _stubManager;

        [SetUp]
        public void SetUp()
        {
            _stubFactory = MockRepository.GenerateStub<ICommandFactory>();
            _stubFactory.Expect(x => x.Contains("echo")).Return(true);
            _stubFactory.Expect(x => x.GetInstance("echo")).Return(new EchoCommand());
            _stubManager = MockRepository.GenerateMock<CommandManager>(_stubFactory);
            _controller = new CommandsController(_stubManager);
        }

        [Test]
        public void Test_GetCommandThrowsHttpResponseException()
        {
            Assert.Throws<HttpResponseException>(() => _controller.GetCommmand("hello"));
        }

        [Test]
        public void Test_PostActionStatusCode()
        {
            SetupControllerForTests(_controller);
            var response = _controller.PostAction("echo:echoed Hello my dear");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public void Test_PostActionContent()
        {
            SetupControllerForTests(_controller);
            var response = _controller.PostAction("echo:echoed Hello my dear");

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
