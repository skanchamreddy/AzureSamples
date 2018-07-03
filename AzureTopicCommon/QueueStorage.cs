using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
namespace AzureTopicCommon
{
  public  class QueueStorage
    {
        static string queueName = "myqueue";       
        public void PushToQueue(FileModel fileModel)
        {
            try
            {
                CloudStorageAccount storageAccount = Common.CreateStorageAccountFromConnectionString();
                CloudQueueClient cloudQueueClient = storageAccount.CreateCloudQueueClient();
                CloudQueue cloudQueue = cloudQueueClient.GetQueueReference(queueName);
                cloudQueue.CreateIfNotExists();

                CloudQueueMessage cloudQueueMessage = new CloudQueueMessage(Common.ObjectToByteArray<FileModel>(fileModel));
               
               cloudQueue.AddMessage(cloudQueueMessage,TimeSpan.FromMinutes(10));
            }
            catch(Exception ex)
            {

                Console.WriteLine(ex);
                Console.ReadLine();
            }
        }
        public FileModel ReadFromQueue()
        {
            FileModel fileMode = null;
            try
            {
                CloudStorageAccount storageAccount = Common.CreateStorageAccountFromConnectionString();
                CloudQueueClient cloudQueueClient = storageAccount.CreateCloudQueueClient();
                CloudQueue cloudQueue = cloudQueueClient.GetQueueReference(queueName);
                CloudQueueMessage peekedMessage = cloudQueue.GetMessage();
                if (peekedMessage?.AsBytes != null)
                {
                    fileMode = Common.ByteArrayToObject<FileModel>(peekedMessage.AsBytes);

                    // QueueRequestOptions queueRequestOptions= new QueueRequestOptions() { RetryPolicy= IRetryPolicy }
                    // cloudQueue.DeleteMessage(peekedMessage.Id, peekedMessage.PopReceipt);
                    cloudQueue.DeleteMessage(peekedMessage);
                }
                return fileMode;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                throw ex;
            }

        }

        public void DeleteAllMessges()
        {
            CloudStorageAccount storageAccount = Common.CreateStorageAccountFromConnectionString();
            CloudQueueClient cloudQueueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue cloudQueue = cloudQueueClient.GetQueueReference(queueName);
           IEnumerable< CloudQueueMessage> peekedMessages = cloudQueue.GetMessages(10);
            foreach(CloudQueueMessage cloudQueueMessage in peekedMessages)
            {
                cloudQueue.DeleteMessage(cloudQueueMessage);
            }
        }
      

    }
}
