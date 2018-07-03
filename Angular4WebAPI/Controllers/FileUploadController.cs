//https://github.com/AliAdravi/Angular-4-upload-files-with-data-and-Web-API

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AzureTopicCommon;
namespace Angular4WebAPI.Controllers
{
    public class FileUploadController : ApiController
    {
        [HttpPost]
        public async Task<FileModel> Upload(int projectId, int sectionId)
        {
            
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            var context = HttpContext.Current.Request;
            var file = new FileModel();
            if (context.Files.Count > 0)
            {
                var filesReadToProvider = await Request.Content.ReadAsMultipartAsync();
                var index = 0;
                BlobStorage blobStorage = new BlobStorage();
                TableStorage tableStorage = new TableStorage();
                QueueStorage queueStorage = new QueueStorage();
               
                foreach (var streamContent in filesReadToProvider.Contents)
                {
                  
                    file.inputStream = await streamContent.ReadAsByteArrayAsync();
                   
                    file.ProjectId = projectId;
                    file.SectionId = sectionId;
                    file.FileName = context.Files[index].FileName;

                    file.FileSize = file.inputStream.Length;
                    file.ImagePath = String.Format("{0}_{1}_{2}_{3}", projectId, sectionId, DateTime.Now.Ticks, file.FileName);
                    file.ThumbPath = String.Format("/UploadedFiles/{0}_{1}_th_{2}_{3}", projectId, sectionId, file.FileName, DateTime.Now.Ticks);
                    //var img = Image.FromStream(new System.IO.MemoryStream(fileBytes));
                    blobStorage.Upload(file);
                    file.inputStream = null;
                    // tableStorage.SaveTableStorage(file);
                    queueStorage.PushToQueue(file);

                  // FileModel fileModel= queueStorage.ReadFromQueue();
                    index++;
                }
            }
                return file;
        }
        [HttpGet]
        public IEnumerable<FileModel> Getimages()
        {
            TableStorage tableStorage = new TableStorage();
            BlobStorage blobStorage = new BlobStorage();
            List<FileModel> fileModels = new List<FileModel>();
            IEnumerable<FileModelTable> fileModelTableList = tableStorage.RetrieveTableStorageData();
            if (fileModelTableList!= null)
            {
                foreach(FileModelTable fileModelTable in fileModelTableList)
                {
                    fileModels.Add(new FileModel() {
                        FileName = fileModelTable.FileName,
                        FileSize = fileModelTable.FileSize,
                        ImagePath = fileModelTable.ImagePath,
                        ProjectId = fileModelTable.ProjectId,
                        SectionId= fileModelTable.SectionId,
                        ThumbPath = fileModelTable.ThumbPath
                    });
                }
            }
            if (fileModels.Count > 0)
            {
                blobStorage.GenerateSASUriToFiles(fileModels);
            }
            return fileModels;
        }
    }
   
}
