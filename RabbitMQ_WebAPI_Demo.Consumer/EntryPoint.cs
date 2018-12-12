﻿using RabbitMQ_WebAPI_Demo.Messages;
using System;

namespace RabbitMQ_WebAPI_Demo.Consumer
{
    public class EntryPoint
    {

        private static readonly string QueueName = "votes-queue";
        private static readonly string RabbitMQServer = "localhost";
        private static readonly int RabbitMQPort = 5672;
        private static readonly string RabbitMQUsername = "admin";
        private static readonly string RabbitMQPassword = "admin";

        static void Main(string[] args)
        {
            Console.WriteLine("Waiting for incoming messages, Queue[{0}], Press Ctrl + C to exit...", QueueName);

            try
            {
                //Process incoming message using RabbitMQ wrapper class - Read Vote Messages
                MessageProcessor.SetConfig(QueueName, RabbitMQServer, RabbitMQPort, RabbitMQUsername, RabbitMQPassword);
                MessageProcessor.ConsumeMessages<Vote>(msg => {
                    Console.WriteLine($"Vote Received at { DateTime.Now.ToString() }: " +
                        $"{{ \"VoteId\": \"{ msg.VoteId }\", " +
                        $"\"DogName\": \"{ msg.DogName }\", " +
                        $"\"UserId\": \"{ msg.UserId }\"}}");
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }

            Console.WriteLine("\n\nPress any key to continue...");
            Console.ReadKey();

        }
    }
}
