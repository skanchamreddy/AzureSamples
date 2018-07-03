using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
namespace AzureTopicCommon
{
    public class BlobStorage
    {
        
        static string storageContainerName = "2018";
        static string storageBlobName = string.Empty;
      

        public void Upload(FileModel fileModel)
        {
            try
            {
                CloudStorageAccount storageAccount = Common.CreateStorageAccountFromConnectionString();
                CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(storageContainerName);
                cloudBlobContainer.CreateIfNotExists();

                BlobContainerPermissions blobContainerPermissions = new BlobContainerPermissions() { PublicAccess = BlobContainerPublicAccessType.Blob };
                cloudBlobContainer.SetPermissions(blobContainerPermissions);
                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileModel.ImagePath);
                cloudBlockBlob.UploadFromByteArray(fileModel.inputStream, 0, fileModel.inputStream.Length);
            }
            catch (Exception ex)
            {

            }
        }
        private string GetBlobSasUri(CloudBlobContainer container, FileModel fileModel )
        {
            //Get a reference to a blob within the container.
            CloudBlockBlob blob = container.GetBlockBlobReference(fileModel.ImagePath);
            SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy();
            sasConstraints.SharedAccessStartTime = DateTimeOffset.UtcNow.AddMinutes(-5);
            sasConstraints.SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddHours(1);
            sasConstraints.Permissions = SharedAccessBlobPermissions.Read;
            string sasBlobToken = blob.GetSharedAccessSignature(sasConstraints);
            return blob.Uri + sasBlobToken;
        }
        public void GenerateSASUriToFiles(List<FileModel> fileModels)
        {
            CloudStorageAccount storageAccount = Common.CreateStorageAccountFromConnectionString();
            CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(storageContainerName);

            foreach (FileModel fileModel in fileModels)
            {
                fileModel.ImagePath= GetBlobSasUri(cloudBlobContainer, fileModel);
            }
        }
    }
}
