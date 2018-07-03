using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AzureTopicCommon;
namespace QueuesGettingStarted
{
    class Program
    {
        static void Main(string[] args)
        {
            //int j;
            ////int[] array = new int[10];
            //for ( int i=0;i<=10;++i)
            //{
            //  j=  ++i;
            //    Console.WriteLine(i);
            //    Console.WriteLine(j);
            //    Console.WriteLine("==================");
            //    j =   i++;
            //    Console.WriteLine(i);
            //    Console.WriteLine(j);
            //}
            //Console.ReadLine();
            string Clientid = "42c76f2f-fd88-4d6f-a385-06273df9192a";
            string ApplicationSecret = "zY5JuVvolq1Z/h3H9UBCPrTNxKsjRsWVBQctDUt33mw=";
            //string ScretIdenfier = @"https://mycdp-keyvalt.vault.azure.net/secrets/my-storage/fc22e81e4d854388a30193c064578f43";
            //KeyVault keyVault =new KeyVault(Clientid, ApplicationSecret, ScretIdenfier);
            //var value= keyVault.GetKeyValue().Result;

           // Console.WriteLine($"value=> {value}  ");
            Console.WriteLine("started.......");
            while (true)
            {
                QueueStorage queueStorage = new QueueStorage();
                TableStorage tableStorage = new TableStorage();
                FileModel fileModel = queueStorage.ReadFromQueue();
                if (fileModel != null)
                {
                    Console.WriteLine("Sucessfully read the data from Queue");
                    FileModelTable fileModelTable = new FileModelTable()
                    {
                        FileName = fileModel.FileName,
                        FileSize = fileModel.FileSize,
                        ImagePath = fileModel.ImagePath,
                        ProjectId = fileModel.ProjectId,
                        SectionId = fileModel.SectionId,
                        ThumbPath = fileModel.ThumbPath
                    };
                    tableStorage.SaveTableStorage(fileModelTable);
                    Console.WriteLine("Sucessfully save data to Table");
                    
                }
                Thread.Sleep(5000);
            }
            Console.ReadLine();
        }
    }
}
