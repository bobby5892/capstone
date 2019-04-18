using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using PeerIt.Models;
using PeerIt.Repositories;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PeerIt.Controllers
{
    ///
    public class PFileController : Controller
    {
        private readonly IFileProvider _fileProvider;
        IHostingEnvironment _hostingEnvironment;
        private PFileRepository _fileRepository;
        ///
        public PFileController(IHostingEnvironment hostingEnvironment, IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
            _hostingEnvironment = hostingEnvironment;
            //_fileRepository = fileRepository;
        }
        ///
        [HttpPost]
        public async Task<IActionResult> Post(List<IFormFile> files)
        {
            _fileRepository = new PFileRepository();
            PFile theFile;
            Stream stream;
            DirectoryInfo dataFolder = new DirectoryInfo("Data/temp.png");
      
            // full path to file in temp location
            var filePath = Path.GetTempFileName();
            //System.IO.File.Copy(filePath, destinationFolder);
            long size = files.Sum(f => f.Length);
            foreach (var formFile in files)
            {
                string destinationFolder = "Data/" + formFile.FileName;
             //

                theFile = new PFile();

                if (formFile.Length > 0)
                {
                    using (stream = new FileStream(destinationFolder, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);           
                    }
                    //StreamWriter streamWriter = new StreamWriter()
                    
                    //StreamWriter outputFile = new StreamWriter()
                }
                
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = files.Count, size, filePath });
        }
    }
}
