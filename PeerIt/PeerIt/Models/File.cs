using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PeerIt.Models
{
    public class File : IFormFile
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Content type of file
        /// </summary>
        public string ContentType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ContentDisposition { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IHeaderDictionary Headers { get; set; }
        /// <summary>
        /// In Bytes how long is the file
        /// </summary>
        public long Length { get; set; }
        /// <summary>
        /// Description of file
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// What the full filename is
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        ///  Where the file is saved
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// The File for the database
        /// </summary>
        public byte TheFile { get; set; }
        
        
        /// Save
        public void CopyTo(Stream target)
        {
       
            
        }
        /// Async Save
        public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
        ///Read
        public Stream OpenReadStream()
        {
            throw new NotImplementedException();
        }
    }

}
