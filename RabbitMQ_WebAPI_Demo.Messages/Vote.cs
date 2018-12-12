using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ_WebAPI_Demo.Messages
{
    public class Vote
    {
        public string VoteId { get; set; }
        public string DogName { get; set; }
        public string UserId { get; set; }
    }
}
