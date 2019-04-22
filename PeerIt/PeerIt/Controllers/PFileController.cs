using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using PeerIt.Interfaces;
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
        private IGenericRepository<PFile, int> pFileRepo;
        private List<PFile> pFiles;
        ///
        public PFileController(IHostingEnvironment hostingEnvironment, IFileProvider fileProvider, IGenericRepository<PFile,int> repo)
        {
            _fileProvider = fileProvider;
            _hostingEnvironment = hostingEnvironment;
            pFileRepo = repo;
        }
        ///
        [HttpPost]
        public async Task<IActionResult> Post(List<IFormFile> files)
        { 
            PFile theFile;
            Stream stream;
            // full path to file in temp location
            //var filePath = Path.GetTempFileName();
            //System.IO.File.Copy(filePath, destinationFolder);
            long size = files.Sum(f => f.Length);
            foreach (var formFile in files)
            {
                theFile = new PFile();
                pFileRepo.Add(theFile);
                pFiles = pFileRepo.GetAll();
                Guid fileID = new Guid();
                fileID.ToString();
                //PFile dbpFile = pFileRepo.FindByID(pFiles.Count-1);
                //string fileName = dbpFile.ID.ToString();
                string destinationFolder = "Data/somemsturid";


                if (formFile.Length > 0)
                {
                    using (stream = new FileStream(destinationFolder, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);           
                    }
                } 
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = files.Count, size,  }); //filePath
        }
    }
}
