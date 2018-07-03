using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
namespace AzureTopicCommon
{
    [Serializable]
   public class FileModel
    {
        public FileModel()
        {
            //this.PartitionKey =$"{DateTime.Now.Year.ToString()} - {DateTime.Now.Month.ToString()}";
            //this.RowKey = DateTime.Now.Day.ToString();
        }
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public long contentLenght { get; set; }
        [DataMember]
        public Byte[] inputStream { get; set; }
        [DataMember]
        public int ProjectId { get; set; }
        [DataMember]
        public int SectionId { get; set; }
        [DataMember]
        public int FileSize { get; set; }
        [DataMember]
        public string ImagePath { get; set; }
        [DataMember]
        public string ThumbPath { get; set; }
    }
    public class FileModelTable:TableEntity
    {

        public FileModelTable()
        {
            this.PartitionKey = $"{DateTime.Now.Year.ToString()} - {DateTime.Now.Month.ToString()}";
            this.RowKey = DateTime.Now.Ticks.ToString();
        }
        public string FileName { get; set; }
        public long contentLenght { get; set; }

        public Byte[] inputStream { get; set; }

        public int ProjectId { get; set; }
        public int SectionId { get; set; }
        public int FileSize { get; set; }
        public string ImagePath { get; set; }
        public string ThumbPath { get; set; }
    }
}
