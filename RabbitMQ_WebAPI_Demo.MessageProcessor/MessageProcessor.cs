using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMQ_WebAPI_Demo
{
    public static class MessageProcessor
    {

        private static string QueueName;
        private static string RabbitMQServer;
        private static int RabbitMQPort;
        private static string RabbitMQUsername;
        private static string RabbitMQPassword;
        private static ConnectionFactory factory = null;

        private static void BuildConnection()
        {
            if (factory == null)
            {
                // Set configuration to connect to RabbitMQ Server
                factory = new ConnectionFactory() { HostName = RabbitMQServer, Port = RabbitMQPort, UserName = RabbitMQUsername, Password = RabbitMQPassword };
            }
        }

        private static byte[] GetMessageByteArray<T>(T message)
        {
            // Get message body from T
            String messageToSend = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(messageToSend);

            return body;
        }

        private static String GetMessageString(BasicDeliverEventArgs ea)
        {
            var bodyMesage = ea.Body;
            var message = Encoding.UTF8.GetString(bodyMesage);

            return message;
        }

        public static void SetConfig(string _QueueName, string _RabbitMQServer, int _RabbitMQPort, string _RabbitMQUsername, string _RabbitMQPassword)
        {
            QueueName = _QueueName;
            RabbitMQServer = _RabbitMQServer;
            RabbitMQPort = _RabbitMQPort;
            RabbitMQUsername = _RabbitMQUsername;
            RabbitMQPassword = _RabbitMQPassword;
        }

        public static bool ProduceMessage<T>(T message)
        {

            bool retVal;

            try
            {
                BuildConnection();

                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        // Declare / Create a Single Queue
                        channel.QueueDeclare(QueueName, false, false, false, null);

                        // Publish / Send simple message to queue
                        channel.BasicPublish("", QueueName, null, GetMessageByteArray(message));
                    }
                }
                retVal = true;
            }
            catch (Exception ex)
            {
                retVal = false;
            }

            return retVal;

        }

        public static void ConsumeMessages()
        {

            try
            {
                BuildConnection();

                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        // Declare / Create / Read a Single Queue
                        channel.QueueDeclare(QueueName, false, false, false, null);
                        var consumer = new QueueingBasicConsumer(channel);
                        channel.BasicConsume(QueueName, true, consumer);

                        // Wait for incoming messages
                        while (true)
                        {
                            var ea = consumer.Queue.Dequeue();
                            var message = GetMessageString(ea);

                            Console.WriteLine($"Vote Received at { DateTime.Now.ToString() }: {message}");
                        }

                    }
                }

            }
            catch (Exception)
            {
                throw new Exception("Error found while reading messages from Queue");
            }

        }

    }
}
