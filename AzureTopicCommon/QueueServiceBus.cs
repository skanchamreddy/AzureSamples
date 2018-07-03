using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
namespace AzureTopicCommon
{
   public class QueueServiceBus
    {
        const string ServiceBusConnectionString = "Endpoint=sb://cdp-azure.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ZHljUkCE3qZiqPS+N1ViHnA6jLAAbdIquEihsIFhLkc=";
        const string QueueName = "";
        IQueueClient queueClient;
        Message message;
        public async Task SendMessagesAsync(int numberOfMessagesToSend)
        {
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
            for (var i = 0; i < numberOfMessagesToSend; i++)
            {
                string messageBody = $"Message {i}";
                Message message = new Message(Encoding.UTF8.GetBytes(messageBody));
                message.ContentType = "string";
                await queueClient.SendAsync(message);
            }
        }

        public async Task SendMessageAsync(string messageBody)
        {
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
            Message message = new Message(Encoding.UTF8.GetBytes(messageBody));
            message.ContentType = "string";
            await queueClient.SendAsync(message);
        }
        public async Task SendObjectAsync<T>(T t)
        {
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
            Message message = new Message(Common.ObjectToByteArray<T>(t));
            message.ContentType = typeof(T).Name;
            await queueClient.SendAsync(message);
        }
        public async Task ReceiveMessage()
        {
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);

            MessageHandlerOptions messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                AutoComplete = false,
                MaxConcurrentCalls=1
            
            };
            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }
        public  Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }
        private async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber}");
            // Complete the message so that it is not received again.
            // This can be done only if the queue Client is created in ReceiveMode.PeekLock mode (which is the default).
            if (message.ContentType == "string")
            {
                Console.WriteLine($"Received message: body:{Encoding.UTF8.GetString(message.Body)}");
            }
            else if (message.ContentType.ToLower() == "filemodel")
            {
                FileModel fileMode = Common.ByteArrayToObject<FileModel>(message.Body);
                TableStorage tableStorage = new TableStorage();
                FileModelTable fileModelTable = new FileModelTable()
                {
                    FileName = fileMode.FileName,
                    FileSize = fileMode.FileSize,
                    ImagePath = fileMode.ImagePath,
                    ProjectId = fileMode.ProjectId,
                    SectionId = fileMode.SectionId,
                    ThumbPath = fileMode.ThumbPath
                };
                tableStorage.SaveTableStorage(fileModelTable);

            }
            await queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }
    }
}
