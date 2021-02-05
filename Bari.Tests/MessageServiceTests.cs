using Bari.Domain.Entities;
using Bari.Domain.Interfaces;
using Bari.Domain.Services;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using Serilog;
using System;

namespace Bari.Tests
{
    public class MessageServiceTests
    {
        protected MessageService MessageService { get; set; }
        protected Mock<ILogger> Logger { get; set; }
        protected Mock<ISettings> Settings { get; set; }

        [SetUp]
        public void Setup()
        {
            Logger = new Mock<ILogger>();
            Settings = new Mock<ISettings>();

            MessageService = new MessageService(Logger.Object, Settings.Object);
        }

        [TearDown]
        public void TearDown()
        {
            Logger.VerifyAll();
            Settings.VerifyAll();
        }
    }

    public class Proccess : MessageServiceTests
    {
        [Test]
        public void ShouldLoggerCorrectly()
        {
            var message = new Message
            {
                Date = DateTime.Now,
                Id = Guid.NewGuid().ToString(),
                ServiceName = "Test",
                Value = "Hello"
            };

            MessageService.Proccess(JsonConvert.SerializeObject(message));

            Logger.Verify(m => m.Information(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<string>()), Times.Once());
        }

        [Test]
        public void ShouldLoggerMessageCorrectly()
        {
            var message = new Message
            {
                Date = DateTime.Now,
                Id = Guid.NewGuid().ToString(),
                ServiceName = "Test",
                Value = "Hello"
            };

            MessageService.Proccess(JsonConvert.SerializeObject(message));

            Logger.Verify(m => m.Information(It.Is<string>(x => x == "ServiceName:{ServiceName}, Id:{Id}, Date:{Date}, Value:{Value}"), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<string>()), Times.Once());
        }
    }

    public class Create : MessageServiceTests
    {
        [Test]
        public void ShouldCreateDateCorrectly()
        {
            var result = MessageService.Create("Hello");

            Assert.IsTrue(result.Date > DateTime.MinValue);
        }

        [Test]
        public void ShouldCreateIdCorrectly()
        {
            var result = MessageService.Create("Hello");

            Assert.IsNotNull(result.Id);
        }

        [Test]
        public void ShouldCreateServiceNameCorrectly()
        {
            var serviceName = "Bari";
            Settings.Setup(x => x.ServiceName).Returns(serviceName);

            var result = MessageService.Create("Hello");

            Assert.AreEqual(serviceName, result.ServiceName);
        }

        [Test]
        public void ShouldCreateValueCorrectly()
        {
            var value = "Hello Word";
            
            var result = MessageService.Create(value);

            Assert.AreEqual(value, result.Value);
        }
    }
}