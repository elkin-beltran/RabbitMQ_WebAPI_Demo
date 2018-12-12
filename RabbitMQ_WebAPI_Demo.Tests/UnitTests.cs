using NUnit.Framework;
using RabbitMQ_WebAPI_Demo.Messages;

namespace RabbitMQ_WebAPI_Demo.Tests
{
    [TestFixture]
    public class UnitTests
    {

        private string QueueName;
        private string RabbitMQServer;
        private int RabbitMQPort;
        private string RabbitMQUsername;
        private string RabbitMQPassword;

        [SetUp]
        public void Init()
        {
            QueueName = "votes-queue";
            RabbitMQServer = "localhost";
            RabbitMQPort = 5672;
            RabbitMQUsername = "admin";
            RabbitMQPassword = "admin";
        }

        [Test]
        public void Inject_VoteIntoQueue_OK()
        {
            //// Arrange
            Vote vote = new Vote() { VoteId = "1", DogName = "Fuffy", UserId = "456465-121212-454354-789123"};

            //// Act
            MessageProcessor.Disconnect();
            MessageProcessor.SetConfig(QueueName, RabbitMQServer, RabbitMQPort, RabbitMQUsername, RabbitMQPassword);
            var result = MessageProcessor.ProduceMessage<Vote>(vote);

            //// Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Inject_VoteIntoQueue_KO()
        {
            //// Arrange
            RabbitMQPort = 25672;
            Vote vote = new Vote() { VoteId = "9999", DogName = "Pluto", UserId = "789123-121212-456465-454354" };

            //// Act
            MessageProcessor.Disconnect();
            MessageProcessor.SetConfig(QueueName, RabbitMQServer, RabbitMQPort, RabbitMQUsername, RabbitMQPassword);
            var result = MessageProcessor.ProduceMessage<Vote>(vote);

            //// Assert
            Assert.IsFalse(result);
        }

    }
}
