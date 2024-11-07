using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Models;
using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Processors;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Text;

namespace AlaskaAir.MileagePlan.OneWorldProfileSync.Function.UnitTests.Processors
{
    [TestFixture]
    class ServiceBusMessageProcessorTests
    {
        Message message;

        [SetUp]
        public void Init()
        {
            message = new Message
            {
                Body = Encoding.UTF8.GetBytes("{\"BankCardDesig\": \"Signature\"}")
            };
        }

        [Test]
        public void GetMessage_Returns_Deserialized_SolarTransactionResponse()
        {
            // Act
            var profilePublisherRequest = ServiceBusMessageProcessor.GetMessage(message);

            // Assert
            Assert.AreEqual("Signature", profilePublisherRequest.BankCardDesig);
        }

        [Test]
        public void ReQueueMessage_Calls_Add_Method()
        {
            // Arrange
            Mock<ICollector<Message>> mock = new Mock<ICollector<Message>>();

            // Act
            Message retriedMessage = ServiceBusMessageProcessor.ReQueueMessage(message, mock.Object);

            // Assert
            mock.Verify(x => x.Add(It.IsAny<Message>()), Times.Once);

            Assert.AreEqual(message.Body, retriedMessage.Body);
            Assert.AreEqual(1, retriedMessage.UserProperties["retryCount"]);
            Assert.IsTrue(retriedMessage.ScheduledEnqueueTimeUtc > DateTime.UtcNow.AddMinutes(4));

            // Act
            retriedMessage = ServiceBusMessageProcessor.ReQueueMessage(retriedMessage, mock.Object);

            // Assert
            mock.Verify(x => x.Add(It.IsAny<Message>()), Times.Exactly(2));

            Assert.AreEqual(message.Body, retriedMessage.Body);
            Assert.AreEqual(2, retriedMessage.UserProperties["retryCount"]);
            Assert.IsTrue(retriedMessage.ScheduledEnqueueTimeUtc > DateTime.UtcNow.AddMinutes(2 * 4));
        }

        [Test]
        public void SendMessageToTopic_Calls_Add_method()
        {
            // Arrange
            Mock<ICollector<string>> mock = new Mock<ICollector<string>>();

            ProfilePublisherRequest profilePublisherRequest = new ProfilePublisherRequest
            {
                BankCardDesig = "Signature"
            };

            // Act
            ServiceBusMessageProcessor.SendMessageToTopic(profilePublisherRequest, mock.Object);

            // Assert
            mock.Verify(x => x.Add(JsonConvert.SerializeObject(profilePublisherRequest)), Times.Once);

        }
    }
}
