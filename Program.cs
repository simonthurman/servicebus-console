// See https://aka.ms/new-console-template for more information
using System;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;

namespace servicebus
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Set the connection string and queue name
            string connectionString = "<connection_string>";
            string queueName = "myqueue";

            // Create a Service Bus Admin client to create a queue
            var admidClient = new ServiceBusAdministrationClient(connectionString);

            // Create a queue if not exists
            bool queueExists = await admidClient.QueueExistsAsync(queueName);
            if (!queueExists)
            {
                await admidClient.CreateQueueAsync(new CreateQueueOptions(queueName));
            }
            
            // Create a Service Bus client to manipulate the queue
            var myClient = new ServiceBusClient(connectionString);
            // Create queue
            var sender = myClient.CreateSender(queueName);

            // Create Service Bus Options client to set options 
            var clientOptions = new ServiceBusClientOptions();
            // Set AMQP protocol
            clientOptions.TransportType = ServiceBusTransportType.AmqpWebSockets;

            // Create Service Bus message
            var message = new ServiceBusMessage("Hello World123!");
            // Send message
            await sender.SendMessageAsync(message);


            // Create a Service Bus receiver
            var receiver = myClient.CreateReceiver(queueName);
            // Receive message
            var receivedMessage = await receiver.ReceiveMessageAsync();
            
            // Print message
            Console.WriteLine(receivedMessage.Body.ToString());
            
            //delete message
            await receiver.CompleteMessageAsync(receivedMessage);

        }
    }
}