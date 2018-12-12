using RabbitMQ_WebAPI_Demo.Messages;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RabbitMQ_WebAPI_Demo.Producer.Controllers
{
    public class VoteController : ApiController
    {

        private readonly string QueueName = "votes-queue";
        private readonly string RabbitMQServer = "localhost";
        private readonly int RabbitMQPort = 5672;
        private readonly string RabbitMQUsername = "admin";
        private readonly string RabbitMQPassword = "admin";

        // POST api/values
        public HttpResponseMessage Post([FromBody]Vote vote)
        {

            //Process incoming message using RabbitMQ wrapper class
            MessageProcessor.SetConfig(QueueName, RabbitMQServer, RabbitMQPort, RabbitMQUsername, RabbitMQPassword);
            MessageProcessor.ProduceMessage<Vote>(vote);

            return Request.CreateResponse(HttpStatusCode.Created);
        }

    }
}
