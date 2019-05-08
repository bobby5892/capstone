using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PeerIt.Models;
using PeerIt.Repositories;
using Newtonsoft.Json;
using PeerIt.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.IO;
using PeerIt.Interfaces;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Hosting;

namespace PeerIt.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ReviewController : Controller
    {
        #region Private Variables

        private readonly IFileProvider _fileProvider;
        private IHostingEnvironment _hostingEnvironment;
        private IGenericRepository<StudentAssignment, int> studentAssignmentRepo;
        private IGenericRepository<Review, int> reviewRepo;
        private IGenericRepository<PFile, string> pFileRepo;
        private IGenericRepository<CourseGroup, int> courseGroupRepo;
        private PFile downloadFile;
        private StudentAssignment studentAssignment;
        private JsonResponse<Review> response;
        private UserManager<AppUser> userManager;

        private bool isAdmin;
        private bool isInstructor;
        private bool isStudent;

        #endregion Private Variables

        #region Constructors


        public ReviewController(UserManager<AppUser> userMgr,
                                IGenericRepository<CourseGroup, int> cgRepo,
                                IGenericRepository<Review, int> rRepo,
                                IGenericRepository<PFile,string> pRepo,
                                IGenericRepository<StudentAssignment, int> sRepo,
                                IFileProvider fProvider,
                                IHostingEnvironment hostEnvironment)
        {
            userManager = userMgr;
            _hostingEnvironment = hostEnvironment;
            _fileProvider = fProvider;
            reviewRepo = rRepo;
            pFileRepo = pRepo;
            studentAssignmentRepo = sRepo;
            courseGroupRepo = cgRepo;

            this.isAdmin = HttpContext.User.IsInRole("Administrator");
            this.isInstructor = HttpContext.User.IsInRole("Instructor");
            this.isStudent = HttpContext.User.IsInRole("Student");
        }

        #endregion Constructors

        #region Methods That Return Json
        public async Task<JsonResult> GetReviewsByAssignmentId(int assignmentId)
        {
            response = new JsonResponse<Review>();
            studentAssignment = studentAssignmentRepo.FindByID(assignmentId);
            List<Review> reviews = reviewRepo.GetAll();
            AppUser user = await userManager.GetUserAsync(HttpContext.User);

            if (studentAssignment != null)
            {
                if (this.isAdmin || this.isInstructor && studentAssignment.CourseAssignment.FK_COURSE.FK_INSTRUCTOR.Id == user.Id)
                {
                    foreach (Review r in reviews)
                    {
                        if (r.FK_STUDENT_ASSIGNMENT == studentAssignment)
                        {
                            response.Data.Add(r);
                        }
                    }
                    return Json(response);
                }
                else if (this.isStudent)
                {
                    foreach (Review r in reviews)
                    {
                        if (r.FK_APP_USER == user && r.FK_STUDENT_ASSIGNMENT == studentAssignment)
                        {
                            response.Data.Add(r);
                        }
                    }
                    return Json(response);
                }
                else
                {
                    response.Error.Add(new Error("Forbidden", "You are not allowed here naive."));
                }
            }
            else
            {
                response.Error.Add(new Error("No assignment ", "The student assignment was not found"));
            }
            return Json(response);
        }

        public async Task<JsonResult> GetReviewById(int id)
        {
            JsonResponse<Review> response = new JsonResponse<Review>();
            Review review = reviewRepo.FindByID(id);
            AppUser user = await userManager.GetUserAsync(HttpContext.User);

            if (review != null)
            {
                if (this.isAdmin || this.isInstructor && review.FK_STUDENT_ASSIGNMENT.CourseAssignment.FK_COURSE.FK_INSTRUCTOR == user)
                {
                    response.Data.Add(review);
                    return Json(response);
                }
                else if (this.isStudent)
                {
                    if (review.FK_APP_USER == user)
                    {
                        response.Data.Add(review);
                        return Json(response);
                    }
                    else if (review.FK_STUDENT_ASSIGNMENT.AppUser == user)
                    {
                        response.Data.Add(review);
                        return Json(response);
                    }
                    else
                    {
                        response.Error.Add(new Error("Forbidden", "This Review is not for you or by you."));
                    }
                }
                else
                {
                    response.Error.Add(new Error("Forbidden", "You are not allowed here naive."));
                }
            }
            else
            {
                response.Error.Add(new Error("NotFound", "The review was not found."));
            }
            return Json(response);
        }

        public async Task<JsonResult> CreateReview(Review review)
        {
            AppUser user = await userManager.GetUserAsync(HttpContext.User);
            studentAssignment = studentAssignmentRepo.FindByID(review.FK_STUDENT_ASSIGNMENT.ID);
            Review newReview = new Review();

            if (studentAssignment != null)
            {
                if (this.isAdmin || this.isInstructor && studentAssignment.CourseAssignment.FK_COURSE.FK_INSTRUCTOR == user)
                {
                    newReview = reviewRepo.Add(review);
                    if (newReview != null)
                    {
                        response.Data.Add(newReview);
                    }
                    else
                    {
                        response.Error.Add(new Error("Not Successful", "the review was not successfully added to the database."));
                    }
                }
                else if (this.isStudent)
                {
                    CourseGroup courseGroupForAssignmentBeingReviewed = courseGroupRepo.FindByID(review.FK_STUDENT_ASSIGNMENT.CourseAssignment.FK_COURSE.ID);

                    if (await CurrentUserIsInSameReviewGroup(courseGroupForAssignmentBeingReviewed))
                    {
                        newReview = reviewRepo.Add(review);
                        if (newReview != null)
                        {
                            response.Data.Add(newReview);
                        }
                        else
                        {
                            response.Error.Add(new Error("NotSuccessful", "the data was not successfully written."));
                        }
                    }
                    else
                    {
                        response.Error.Add(new Error("Forbidden", "you are not in this review group."));
                    }
                }
                else
                {
                    response.Error.Add(new Error("Forbidden", "You are not allowed here naive."));
                }
            }
            else
            {
                response.Error.Add(new Error("NotFound", "The assignment was not found."));
            }
            return Json(response);
        }


        [HttpPost]
        public async Task<JsonResult> UploadReview(List<IFormFile> files, int studentAssignmentId)
        {
            PFile newPFile;
            StudentAssignment studentAssignment;
            Review newReview;
            Stream stream;
            Guid guidFileId;
            long size = files.Sum(f => f.Length);
            foreach (var formFile in files)
            {
                guidFileId = Guid.NewGuid();
                string ext = formFile.FileName.Split(".")[1];
                string name = formFile.FileName.Split(".")[0];
                AppUser user = await userManager.GetUserAsync(HttpContext.User);
                studentAssignment = studentAssignmentRepo.FindByID(studentAssignmentId);

                string destinationFolder = "Data/" + guidFileId + "." + ext;


                if (formFile.Length > 0)
                {
                    using (stream = new FileStream(destinationFolder, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    newPFile = new PFile(guidFileId.ToString(), name, ext, user);
                    pFileRepo.Add(newPFile);
                    newReview = new Review() { FK_STUDENT_ASSIGNMENT = studentAssignment, FK_APP_USER = user, FK_PFile = newPFile, TimestampCreated = System.DateTime.Now };

                    string jsonReview = JsonConvert.SerializeObject(await CreateReview(newReview));
                    newReview = JsonConvert.DeserializeObject<Review>(jsonReview);
                    response.Data.Add(newReview);
                }
                else
                {
                    response.Error.Add(new Error("No File", "No file to upload"));
                    return Json(response);
                }
            }
            return Json(response);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadReview(string pFileId)
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

            string downFileName = downloadFile.Name + "." + downloadFile.Ext;
            return File(memory, GetContentType(), downFileName);
        }

        ///Delete a file
        [HttpDelete]
        public JsonResult DeleteReview(string id)
        {
            JsonResponse<string> jsonResponse = new JsonResponse<string>();
            downloadFile = pFileRepo.FindByID(id);
            System.IO.File.Delete("Data/" + downloadFile.ID + "." + downloadFile.Ext);

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
            var ext = "." + downloadFile.Ext;
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
        public async Task<bool> CurrentUserIsInSameReviewGroup(CourseGroup courseGroup)
        {
            AppUser user = await userManager.GetUserAsync(HttpContext.User);
            string reviewGroupForAssignment = courseGroup.ReviewGroup;
            string reviewGroupForUser = "";

            List<CourseGroup> courseGroups = courseGroupRepo.GetAll();
            //List<CourseGroup> courseGroupUserIsInForCourse = new List<CourseGroup>();

            foreach(CourseGroup cg in courseGroups)
            {
                if(cg.FK_AppUser == user && cg.ID == courseGroup.ID)
                {
                    reviewGroupForUser = cg.ReviewGroup;
                }
            }
            if (reviewGroupForUser == reviewGroupForAssignment)
                return true;
            else 
            return false;
        }
        #endregion Methods That Return Json
    }
}
