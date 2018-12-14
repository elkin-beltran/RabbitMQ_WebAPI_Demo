using MongoDB.Bson;
using MongoDB.Driver;
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
        private static string MongoDBUrl = null;
        private static string MongoDBDatabaseName = null;
        private static string MongoDBCollectionName = null;
        private static IMongoClient client;
        private static IMongoDatabase database;
        private static IMongoCollection<BsonDocument> collection;

        private static void BuildConnection()
        {
            if (factory == null)
            {
                // Set configuration to connect to RabbitMQ Server
                factory = new ConnectionFactory() { HostName = RabbitMQServer, Port = RabbitMQPort, UserName = RabbitMQUsername, Password = RabbitMQPassword };

                if (!string.IsNullOrEmpty(MongoDBUrl) && 
                    !string.IsNullOrEmpty(MongoDBDatabaseName) && 
                    !string.IsNullOrEmpty(MongoDBCollectionName)) {

                    try
                    {
                        //Set Connection to MongoDB
                        client = new MongoClient(MongoDBUrl);
                        database = client.GetDatabase(MongoDBDatabaseName);
                        collection = database.GetCollection<BsonDocument>(MongoDBCollectionName);
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                }
            }
        }

        private static byte[] GetMessageAsByteArray<T>(T message)
        {
            // Get message body from T
            string messageToSend = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(messageToSend);

            return body;
        }

        private static T GetMessageAsObject<T>(BasicDeliverEventArgs ea)
        {
            var bodyMesage = ea.Body;
            var message = Encoding.UTF8.GetString(bodyMesage);

            var messageObject = JsonConvert.DeserializeObject<T>(message);

            return messageObject;
        }

        public static void SetRabbitMQConfig(string _QueueName, string _RabbitMQServer, int _RabbitMQPort, string _RabbitMQUsername, string _RabbitMQPassword)
        {
            QueueName = _QueueName;
            RabbitMQServer = _RabbitMQServer;
            RabbitMQPort = _RabbitMQPort;
            RabbitMQUsername = _RabbitMQUsername;
            RabbitMQPassword = _RabbitMQPassword;
        }

        public static void SetDBConfig(string _MongoDBUrl,string _MongoDBDatabaseName, string _MongoDBCollectionName)
        {
            MongoDBUrl = _MongoDBUrl;
            MongoDBDatabaseName = _MongoDBDatabaseName;
            MongoDBCollectionName = _MongoDBCollectionName;
        }

        public static void Disconnect()
        {
            factory = null;
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
                        channel.BasicPublish("", QueueName, null, GetMessageAsByteArray(message));
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

        public static void ConsumeMessages<T>(Action<T> outputFunction = null)
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
                            var messageObject = GetMessageAsObject<T>(ea);

                            outputFunction(messageObject);

                            //Insert T into MongoDB
                            if (client != null)
                            {
                                collection.InsertOne(new BsonDocument { { "Datetime", DateTime.Now.ToString() } }.AddRange(messageObject.ToBsonDocument<T>()));
                            }
                        }

                    }
                }

            }
            catch (Exception)
            {
                throw new Exception("Error found while reading messages from Queue!!!");
            }

        }

    }
}
