using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
//using Microsoft.Azure.CosmosDB.Table;
namespace AzureTopicCommon
{
    public class TableStorage
    {

        public void SaveTableStorage(FileModelTable fileModel)
        {
            CloudStorageAccount storageAccount = Common.CreateStorageAccountFromConnectionString();
            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            // Retrieve a reference to the table.
            CloudTable table = tableClient.GetTableReference("File");
            // Create the table if it doesn't exist.
            table.CreateIfNotExists();

            TableOperation tableOperation = TableOperation.Insert(fileModel);
            table.Execute(tableOperation);
        }
        public IEnumerable<FileModelTable> RetrieveTableStorageData()
        {
            CloudStorageAccount storageAccount = Common.CreateStorageAccountFromConnectionString();
            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            // Retrieve a reference to the table.
            CloudTable table = tableClient.GetTableReference("File");
            // Create the table if it doesn't exist.
             table.CreateIfNotExists();

            TableQuery<FileModelTable> fileModelTables = new TableQuery<FileModelTable>();

            
          return  table.ExecuteQuery(fileModelTables);
        }
    }
}
