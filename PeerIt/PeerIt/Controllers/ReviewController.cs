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

            //this.isAdmin = HttpContext.User.IsInRole("Administrator");
            //this.isInstructor = HttpContext.User.IsInRole("Instructor");
            //this.isStudent = HttpContext.User.IsInRole("Student");
        }

        #endregion Constructors

        #region Methods That Return Json

        // Currently working on this.
        /// <summary>
        /// Gets all the reviews with the passed assignment id
        /// </summary>
        /// <param name="assignmentId"></param>
        /// <returns></returns>
        //public async Task<JsonResult> GetReviewsByAssignmentId(int assignmentId)
        //{
        //    JsonResponse<List<Review>> response = new JsonResponse<List<Review>>();
        //    studentAssignment = studentAssignmentRepository.FindByID(assignmentId);
        //    AppUser user = await userManager.GetUserAsync(HttpContext.User);

        //    if (studentAssignment != null)
        //    {
        //        if (this.isAdmin || this.isInstructor && studentAssignment.CourseAssignment.FK_COURSE.FK_INSTRUCTOR.Id == user.Id)
        //        {
        //            List<Review> reviews = reviewRepository.GetReviewsByAssignment(assignmentId);
        //            response.Data.Add(reviews);
        //            return Json(response);
        //        }
        //        else if (this.isStudent)
        //        {
        //            if (studentAssignment.AppUser.Id == user.Id)
        //            {
        //                List<Review> reviews = reviewRepository.GetReviewsByAssignment(assignmentId);
        //                response.Data.Add(reviews);
        //                return Json(response);
        //            }
        //            else
        //            {
        //                response.Error.Add(new Error("forbidden", "This is not your assignment."));
        //            }
        //        }
        //        else
        //        {
        //            response.Error.Add(new Error("Forbidden", "You are not allowed here naive."));
        //        }
        //    }
        //    else
        //    {
        //        response.Error.Add(new Error("NotFound", "The student assignment was not found"));
        //    }
        //    // Add an error to the list (there can be more than one)
        //    response.Error.Add(new Error() { Name = "No Reviews", Description = "No reviews found for that assignment" });
        //    return Json(response);
        //}
        ///// <summary>
        ///// Gets the review at the passed id
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public async Task<JsonResult> GetReviewById(int id)
        //{
        //    JsonResponse<Review> response = new JsonResponse<Review>();
        //    Review review = reviewRepository.FindByID(id);
        //    AppUser user = await userManager.GetUserAsync(HttpContext.User);

        //    if (review != null)
        //    {
        //        if (this.isAdmin || this.isInstructor && review.FK_STUDENT_ASSIGNMENT.CourseAssignment.FK_COURSE.FK_INSTRUCTOR.Id == user.Id)
        //        {
        //            response.Data.Add(review);
        //            return Json(response);
        //        }
        //        else if (this.isStudent)
        //        {
        //            if (review.FK_APP_USER.Id == user.Id)
        //            {
        //                response.Data.Add(review);
        //                return Json(response);
        //            }
        //            else if (review.FK_STUDENT_ASSIGNMENT.AppUser.Id == user.Id)
        //            {
        //                response.Data.Add(review);
        //                return Json(response);
        //            }
        //            else
        //            {
        //                response.Error.Add(new Error("Forbidden", "This Review is not for you or by you."));
        //            }
        //        }
        //        else
        //        {
        //            response.Error.Add(new Error("Forbidden", "You are not allowed here naive."));
        //        }
        //    }
        //    else
        //    {
        //        response.Error.Add(new Error("NotFound", "The review was not found."));
        //    }
        //    return Json(response);
        //}
        ///// <summary>
        ///// Creates a new review and adds it to the database
        ///// </summary>
        ///// <param name="contents"></param>
        ///// <param name="studentAssignmentId"></param>
        ///// <returns></returns>
        //public async Task<JsonResult> CreateReview(string contents, int studentAssignmentId)
        //{
        //    JsonResponse<Review> response = new JsonResponse<Review>();
        //    //AppUser user = await GetCurrentUserById(userId);
        //    AppUser user = await userManager.GetUserAsync(HttpContext.User);
        //    studentAssignment = studentAssignmentRepository.FindByID(studentAssignmentId);
        //    Course course = studentAssignment.CourseAssignment.FK_COURSE;
        //    AppUser assignUser = studentAssignment.AppUser;

        //    if (studentAssignment != null)
        //    {
        //        if (this.isAdmin || this.isInstructor && studentAssignment.CourseAssignment.FK_COURSE.FK_INSTRUCTOR.Id == user.Id)
        //        {
        //            Review review = new Review() {FK_APP_USER = user, FK_STUDENT_ASSIGNMENT = studentAssignment };
        //            Review newReview = reviewRepository.Add(review);
        //            if (newReview != null)
        //            {
        //                response.Data.Add(newReview);
        //            }
        //            else
        //            {
        //                response.Error.Add(new Error("NotSuccessful", "the data was not successfully written."));
        //            }
        //        }
        //        else if (this.isStudent)
        //        {
        //            CourseGroup courseGroupCurrentUser = courseGroupRepository.GetByUserAndCourseID(user.Id, course.ID);
        //            CourseGroup courseGroupAssignmentUser = courseGroupRepository.GetByUserAndCourseID(assignUser.Id, course.ID);
        //            if (courseGroupCurrentUser.ReviewGroup == courseGroupCurrentUser.ReviewGroup)
        //            {
        //                Review review = new Review() { FK_APP_USER = user, FK_STUDENT_ASSIGNMENT = studentAssignment };
        //                Review newReview = reviewRepository.Add(review);
        //                if (newReview != null)
        //                {
        //                    response.Data.Add(newReview);
        //                }
        //                else
        //                {
        //                    response.Error.Add(new Error("NotSuccessful", "the data was not successfully written."));
        //                }
        //            }
        //            else
        //            {
        //                response.Error.Add(new Error("Forbidden", "you are not in this group."));
        //            }
        //        }
        //        else
        //        {
        //            response.Error.Add(new Error("Forbidden", "You are not allowed here naive."));
        //        }
        //    }
        //    else
        //    {
        //        response.Error.Add(new Error("NotFound", "The assignment was not found."));
        //    }
        //    return Json(response);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="files"></param>
        /// <param name="studentAssignmentId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UploadReview(List<IFormFile> files, int studentAssignmentId)
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
                }
                newPFile = new PFile(guidFileId.ToString(), name, ext, user);
                pFileRepo.Add(newPFile);
                newReview = new Review() { FK_STUDENT_ASSIGNMENT = studentAssignment, FK_APP_USER = user, FK_PFile = newPFile, TimestampCreated = System.DateTime.Now };
                reviewRepo.Add(newReview);
            }


            return Ok(new { count = files.Count, size, }); //filePath
        }
        #endregion Methods That Return Json
    }
}
