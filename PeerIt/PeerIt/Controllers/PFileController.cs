﻿using System;
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
using PeerIt.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PeerIt.Controllers
{
    /// <summary>
    /// Controller
    /// </summary>
    public class PFileController : Controller
    {
        private readonly IFileProvider _fileProvider;
        private IHostingEnvironment _hostingEnvironment;
        private IGenericRepository<PFile, string> pFileRepo;
        private List<PFile> pFiles;
        private UserManager<AppUser> userManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="hostingEnvironment"></param>
        /// <param name="fileProvider"></param>
        /// <param name="repo"></param>
        /// <param name="usermger"></param>

        private PFile downloadFile;
        private StudentAssignmentRepository studentAssignRepo;
        ///
        public PFileController(IHostingEnvironment hostingEnvironment, IFileProvider fileProvider, IGenericRepository<PFile, string> repo, UserManager<AppUser> usermgr)

        {
            _fileProvider = fileProvider;
            _hostingEnvironment = hostingEnvironment;
            pFileRepo = repo;
            userManager = usermgr;
        }
        /// <summary>
        /// Upload a File
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            PFile newPFile;
            Stream stream;
            Guid guidFileId;
            long size = files.Sum(f => f.Length);
            foreach (var formFile in files)
            {
                guidFileId = Guid.NewGuid();
                string ext = formFile.FileName.Split(".")[1];
                string name = formFile.FileName.Split(".")[0];
                AppUser user = await userManager.GetUserAsync(HttpContext.User);

                string destinationFolder = "Data/" +guidFileId + "."+ext;


                if (formFile.Length > 0)
                {
                    using (stream = new FileStream(destinationFolder, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);           
                    }
                }
                newPFile = new PFile(guidFileId.ToString(), name, ext, user);
                pFileRepo.Add(newPFile);
                pFiles = pFileRepo.GetAll();
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = files.Count, size,  }); //filePath
        }

        [HttpGet]
        public async Task<IActionResult> Download(string pFileId)
        {
            AppUser user = await userManager.GetUserAsync(HttpContext.User);

            if (pFileId == null)
                return Content("filename not present");
            downloadFile = pFileRepo.FindByID(pFileId);
            if (user != downloadFile.AppUser)
                return Content("file not available to you");
            string pathToFile = "Data/" + downloadFile.ID + "." + downloadFile.Ext;
            Stream memory = new MemoryStream();

            using (var stream = new FileStream(pathToFile, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            string downFileName = downloadFile.Name +"."+ downloadFile.Ext;
            return File(memory, GetContentType(),downFileName);
        }

        ///Delete a file
        [HttpDelete]
        public JsonResult DeleteFile(string id)
        {
            JsonResponse<string> jsonResponse = new JsonResponse<string>();
            downloadFile = pFileRepo.FindByID(id);
            System.IO.File.Delete("Data/" + downloadFile.ID +"."+ downloadFile.Ext);

            if (pFileRepo.Delete(downloadFile) == true)
            {
                jsonResponse.Data.Add("File Deleted");
                return Json(jsonResponse);
            }
            jsonResponse.Error.Add(new Error() { Name = "Not Deleted", Description = "File not deleted" });
            return Json(jsonResponse);
        }

        ///Helpers
        private string GetContentType()
        {
            var types = GetMimeTypes();
            var ext = "."+downloadFile.Ext;
            return types[ext];
        }


        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
        ///// <summary>
        ///// Upload a review form
        ///// </summary>
        ///// <param name="files"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public async Task<IActionResult> UploadReview(List<IFormFile> files)
        //{
        //    PFile newPFile;
        //    Review newReview();
        //    Stream stream;
        //    Guid guidFileId;
        //    long size = files.Sum(f => f.Length);
        //    foreach (var formFile in files)
        //    {
        //        guidFileId = Guid.NewGuid();
        //        string ext = formFile.FileName.Split(".")[1];
        //        string name = formFile.FileName.Split(".")[0];
        //        AppUser user = await userManager.GetUserAsync(HttpContext.User);

        //        string destinationFolder = "Data/" + guidFileId + "." + ext;


        //        if (formFile.Length > 0)
        //        {
        //            using (stream = new FileStream(destinationFolder, FileMode.Create))
        //            {
        //                await formFile.CopyToAsync(stream);
        //            }
        //        }
        //        newPFile = new PFile(guidFileId.ToString(), name, ext, user);
        //        newPFile.StudentAssignment = studentAssignRepo.FindByID(studentAssignmentId);
        //        newReview = new Review() { FK_STUDENT_ASSIGNMENT = stud}
        //        pFileRepo.Add(newPFile);
        //        pFiles = pFileRepo.GetAll();
        //    }

        //    // process uploaded files
        //    // Don't rely on or trust the FileName property without validation.

        //    return Ok(new { count = files.Count, size, }); //filePath
        //}
    }
}
