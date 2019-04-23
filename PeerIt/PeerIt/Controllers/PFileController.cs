using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        private IGenericRepository<PFile, string> pFileRepo;
        private List<PFile> pFiles;
        private UserManager<AppUser> userManager;
        ///
        public PFileController(IHostingEnvironment hostingEnvironment, IFileProvider fileProvider, IGenericRepository<PFile, string> repo, UserManager<AppUser> usermger)
        {
            _fileProvider = fileProvider;
            _hostingEnvironment = hostingEnvironment;
            pFileRepo = repo;
        }
        ///
        [HttpPost]
        public async Task<IActionResult> Post(List<IFormFile> files)
        {
            PFile newPFile;
            Stream stream;
            Guid guidFileId;
            long size = files.Sum(f => f.Length);
            foreach (var formFile in files)
            {
                guidFileId = Guid.NewGuid();
                string ext = formFile.FileName.Split(".")[1];

                string destinationFolder = "Data/" +guidFileId + "."+ext;


                if (formFile.Length > 0)
                {
                    using (stream = new FileStream(destinationFolder, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);           
                    }
                }
                newPFile = new PFile(guidFileId.ToString(), ext, await userManager.GetUserAsync(HttpContext.User));
                pFileRepo.Add(newPFile);
                pFiles = pFileRepo.GetAll();
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = files.Count, size,  }); //filePath
        }
    }
}
