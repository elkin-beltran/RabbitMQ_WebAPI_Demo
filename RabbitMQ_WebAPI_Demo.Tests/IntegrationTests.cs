using NUnit.Framework;
using RabbitMQ_WebAPI_Demo.Messages;
using System.Net.Http;

namespace RabbitMQ_WebAPI_Demo.Tests
{
    [TestFixture]
    public class IntegrationTests
    {

        [SetUp]
        public void Init()
        {

        }

        [Test]
        public void SendRequestPost_VoteInBody_OK()
        {
            //// Arrange
            Vote vote = new Vote() { VoteId = "2", DogName = "Titan", UserId = "115487-365145-998655-357481" };
            var controller = new Producer.Controllers.VoteController();
            controller.Request = new HttpRequestMessage() { Method = HttpMethod.Post };

            //// Act
            var actionResult = controller.Post(vote);

            //// Assert
            Assert.IsTrue(actionResult.IsSuccessStatusCode);
            Assert.AreEqual(actionResult.ReasonPhrase.Trim().ToLower(), "created".Trim().ToLower());
        }

        [Test]
        public void SendRequestPost_VoteNotInBody_KO()
        {
            //// Arrange
            Vote vote = new Vote() { VoteId = "2", DogName = "Titan", UserId = "115487-365145-998655-357481" };
            var controller = new Producer.Controllers.VoteController();
            controller.Request = new HttpRequestMessage() { Method = HttpMethod.Post };

            //// Act
            var actionResult = controller.Post(null);

            //// Assert
            Assert.IsFalse(actionResult.IsSuccessStatusCode);
            Assert.AreNotEqual(actionResult.ReasonPhrase.Trim().ToLower(), "created".Trim().ToLower());
        }

    }
}
