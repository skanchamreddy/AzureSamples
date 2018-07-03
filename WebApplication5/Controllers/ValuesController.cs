using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication5.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        [HttpOptions]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public FileCustome Get(int id)
        {
           return new FileCustome() {FileId=1,FileName="sdfsadf",FileType=FileType.pdf };
            //return "value";
        }
       
        // POST api/values
        /// <summary>
        /// CustomPost
        /// </summary>
        /// <param name="File"> FileType 0:xml, 1:pdf</param>
        /// <returns></returns>
        public FileCustome Post(FileCustome File)
        {
            return File;
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
        [HttpPost]
        //[Route("CustomPost")]
        public FileCustome CustomPost(FileCustome File)
        {
            return File;
        }
    }

    public class FileCustome
    {
        public int FileId;
        public string FileName;
        public FileType FileType;
    }
    public enum FileType
    {
        xml,
        pdf,
        word

    }
}
