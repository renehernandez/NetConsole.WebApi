using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetConsole.Core.Commands;
using NetConsole.Core.Interfaces;
using NetConsole.WebApi.Exceptions;
using NetConsole.WebApi.Factories;
using NetConsole.WebApi.Interfaces;
using NetConsole.WebApi.Metadata;
using NetConsole.WebApi.Repositories;
using NUnit.Framework;
using Rhino.Mocks;

namespace NetConsole.WebApi.Tests
{
    [TestFixture]
    public class CommandMetadataFactoryTest
    {

        private CommandMetadataFactory _factory;


        [SetUp]
        public void SetUp()
        {
            _factory = new CommandMetadataFactory();
        }

        [Test]
        public void Test_ThrowsNullException()
        {
            // Assert
            Assert.Throws<NullCommandException>(() => _factory.RegisterInstanceMetadata(null));
        }

        [Test]
        public void Test_ThrowsDuplicatedException()
        {
            // Arrange
            _factory.RegisterInstanceMetadata(new EchoCommand());

            // Assert
            Assert.Throws<DuplicatedCommandMetadataException>(() => _factory.RegisterInstanceMetadata(new EchoCommand()));
        }

        [Test]
        public void Test_ThrowsMetadataNotFoundException()
        {
            Assert.Throws<CommandMetadataNotFoundException>(() => _factory.GetInstanceMetadata("hello"));
        }

    }
}
