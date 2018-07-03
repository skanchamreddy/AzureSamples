using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
namespace AzureTopicCommon
{
    public static class AzureTpoic
    {
        const string ServiceBusConnectionString = "Endpoint=sb://cdp-azure.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ZHljUkCE3qZiqPS+N1ViHnA6jLAAbdIquEihsIFhLkc=";
        const string TopicName = "Topic1";
        
        static string  SubscriptionName = "Topic1_1";

        static ITopicClient topicClient;
        static ISubscriptionClient subscriptionClient;

        public static async Task SendMessage()
        {
            try
            {
                int numberOfMessgges = 1;
                OpenTopicConnection();
                while (numberOfMessgges <= 100)
                {
                    var messageBody = $"Message {numberOfMessgges}";
                    var message = new Message(Encoding.UTF8.GetBytes(messageBody));
                    Console.WriteLine($"Sending message {messageBody}");
                    await topicClient.SendAsync(message);
                    numberOfMessgges += 1;
                }
                await topicClient.CloseAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);

            }
        }
        public static async Task SendMessage(string messageBody)
        {
            try
            {
                OpenTopicConnection();
                var message = new Message(Encoding.UTF8.GetBytes(messageBody));
                Console.WriteLine($"Sending message {messageBody}");
                await topicClient.SendAsync(message);               
                await topicClient.CloseAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

            }
        }
        private static void OpenTopicConnection()
        {
            topicClient = new TopicClient(ServiceBusConnectionString, TopicName);
        }
        public static async Task ReceiveMessage(string subscriptionName="")
        {
            if (!string.IsNullOrEmpty(subscriptionName))
            {
                SubscriptionName = subscriptionName;
            }
            subscriptionClient = new SubscriptionClient(ServiceBusConnectionString, TopicName, SubscriptionName);
            RegisterOnMessageHandlerAndReceiveMessages();
            Console.ReadKey();

            await subscriptionClient.CloseAsync();
        }
        public static void RegisterOnMessageHandlerAndReceiveMessages()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                // Maximum number of concurrent calls to the callback ProcessMessagesAsync(), set to 1 for simplicity.
                // Set it according to how many messages the application wants to process in parallel.
                MaxConcurrentCalls = 1,
                // Indicates whether MessagePump should automatically complete the messages after returning from User Callback.
                // False below indicates the Complete will be handled by the User Callback as in `ProcessMessagesAsync` below.
                AutoComplete = false
            };
            // Register the function that processes messages.
            subscriptionClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }
        static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            // Process the message.
            Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");

            // Complete the message so that it is not received again.
            // This can be done only if the subscriptionClient is created in ReceiveMode.PeekLock mode (which is the default).
            await subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);

            // Note: Use the cancellationToken passed as necessary to determine if the subscriptionClient has already been closed.
            // If subscriptionClient has already been closed, you can choose to not call CompleteAsync() or AbandonAsync() etc.
            // to avoid unnecessary exceptions.
        }
        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }
    }
}
