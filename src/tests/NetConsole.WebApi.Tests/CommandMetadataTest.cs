using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetConsole.Core.Commands;
using NetConsole.Core.Interfaces;
using NetConsole.WebApi.Extensions;
using NetConsole.WebApi.Metadata;
using NUnit.Framework;

namespace NetConsole.WebApi.Tests
{
    [TestFixture]
    public class CommandMetadataTest
    {
        private ICommand _command;
        private CommandMetadata _meta;

        [SetUp]
        public void SetUp()
        {
            // Arrange
            _command = new PromptCommand();
            _meta = new CommandMetadata(_command);
        }

        [Test]
        public void Test_MetadataValuesFromCommand()
        {       
            // Assert
            Assert.AreEqual(_command.Name, _meta.Name);
            Assert.AreEqual(_command.Overview, _meta.Overview);
        }

        [Test]
        public void Test_ActionsCountMatchActions()
        {
            // Assert
            Assert.AreEqual(_meta.ActionsCount, _meta.ActionsMeta.Length);
        }

        [Test]
        public void Test_MetadataActionsCountsMatchGetAction()
        {
            // Assert
            Assert.AreEqual(_meta.ActionsCount, _command.GetActions().Count());
        }

        [Test]
        public void Test_MetadataActions()
        {
            // Arrange
            var echo = new EchoCommand();
            _meta = new CommandMetadata(echo);
            var actionMeta = _meta.ActionsMeta[0];

            // Assert
            Assert.AreEqual(true, actionMeta.Default);
            Assert.AreEqual("Echoed", actionMeta.Name);
            Assert.AreEqual("String", actionMeta.ReturnType);
            Assert.AreEqual(1, actionMeta.ParamsType.Length);
            Assert.AreEqual("String[]", actionMeta.ParamsType[0]);
        }

    }
}
