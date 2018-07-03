using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace AzureStorageSamples
{
   public class AzureServiceBusQueue
    {
        const string ServiceBusConnectionString = "<your_connection_string>";
        const string QueueName = "<your_queue_name>";
        static QueueClient queueClient;

        static async Task MainAsync()
        {
            const int numberOfMessages = 10;
            queueClient = new QueueClient(ServiceBusConnectionString,QueueName);
            Console.WriteLine("======================================================");
            Console.WriteLine("Press ENTER key to exit after sending all the messages.");
            Console.WriteLine("======================================================");
            // Send messages.
            await SendMessagesAsync(numberOfMessages);
            Console.ReadKey();

            await queueClient.CloseAsync();
        }
        static async Task SendMessagesAsync(int numberOfMessages)
        {
            try
            {
                for (int i = 0; i <= numberOfMessages; i++)
                {
                    string messageBody = $"message {i}";

                    var message = new Message(Encoding.UTF8.GetBytes(messageBody));
                    // Write the body of the message to the console.
                    Console.WriteLine($"Sending message: {messageBody}");

                    // Send the message to the queue.
                    await queueClient.SendAsync(message);

                }
            }
            catch(Exception ex)
            {

            }
        }
    }
}
