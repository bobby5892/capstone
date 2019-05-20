using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PeerIt.Models;
using PeerIt.Repositories;
using PeerIt.Interfaces;
using PeerIt.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace PeerIt.Controllers
{
    /// <summary>
    /// A Controller object that Handles all requests to the server regarding Student Assignments.
    /// </summary>
    public class StudentAssignmentController : Controller
    {
        #region Private Variables

        private readonly IFileProvider fileProvider;
        private IHostingEnvironment hostingEnvironment;
        private IGenericRepository<Course, int> courseRepo;
        private IGenericRepository<StudentAssignment, int> studentAssignmentRepo;
        private IGenericRepository<CourseAssignment, int> courseAssignmentRepo;
        private IGenericRepository<CourseGroup, int> courseGroupRepo;
        private IGenericRepository<Review, int> reviewRepo;
        private IGenericRepository<PFile, string> pFileRepo;
        private UserManager<AppUser> userManager;

        private bool isAdmin;
        private bool isInstructor;
        private bool isStudent;

        #endregion Private Variables

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fProvider"></param>
        /// <param name="hEvironment"></param>
        /// <param name="cRepo"></param>
        /// <param name="saRepo"></param>
        /// <param name="caRepo"></param>
        /// <param name="cgRepo"></param>
        /// <param name="arRepo"></param>
        /// <param name="userMgr"></param>
        public StudentAssignmentController(IFileProvider fProvider,
                                           IHostingEnvironment hEvironment,
                                           IGenericRepository<Course, int> cRepo,
                                           IGenericRepository<StudentAssignment, int> saRepo,
                                           IGenericRepository<CourseAssignment, int> caRepo,
                                           IGenericRepository<CourseGroup, int> cgRepo,
                                           IGenericRepository<Review, int> rRepo,
                                           IGenericRepository<PFile, string> pRepo,
                                           UserManager<AppUser> userMgr)
        {
            fileProvider = fProvider;
            hostingEnvironment = hEvironment;
            courseRepo = cRepo;
            studentAssignmentRepo = saRepo;
            courseAssignmentRepo = caRepo;
            courseGroupRepo = cgRepo;
            reviewRepo = rRepo;
            pFileRepo = pRepo;
            userManager = userMgr;

            this.isAdmin = HttpContext.User.IsInRole("Administrator");
            this.isInstructor = HttpContext.User.IsInRole("Instructor");
            this.isStudent = HttpContext.User.IsInRole("Student");
        }

        #endregion Constructors

        #region Methods that return Json

        /// <summary>
        /// 
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetAssignmentsByCourseId(int courseID)
        {
            AppUser user = await userManager.GetUserAsync(HttpContext.User);
            JsonResponse<StudentAssignment> response = new JsonResponse<StudentAssignment>();
            List<StudentAssignment> studentAssignments = studentAssignmentRepo.GetAll();
            Course course = courseRepo.FindByID(courseID);
            CourseGroup courseGroup = await GetCourseGroup(courseID);

            if (this.isAdmin || this.isInstructor && course.FK_INSTRUCTOR == user)
            {
                if (course != null)
                {
                    foreach(StudentAssignment sa in studentAssignments)
                    {
                        if (sa.CourseAssignment.FK_COURSE == course)
                            response.Data.Add(sa);
                    }
                    if (response.Data.Count == 0)
                        response.Error.Add(new Error("No Assignments", "No Assignments"));
                }
                else
                {
                    response.Error.Add(new Error("NotFound", "Course was not Found."));
                }
            }
            else if (this.isStudent)
            {
                if (course != null)
                {
                    if (courseGroup != null)
                    {
                        foreach(StudentAssignment sa in studentAssignments)
                        {
                            if (sa.CourseAssignment.FK_COURSE == course && sa.AppUser == user)
                                response.Data.Add(sa);
                        }
                        if (response.Data.Count == 0)
                            response.Error.Add(new Error("No Assignments", "No Assignments"));
                    }
                    else
                    {
                        response.Error.Add(new Error("NotFound", "You aren't or weren't enrolled in this course."));
                    }
                }
                else
                {
                    response.Error.Add(new Error("NotFound", "Course was not Found."));
                }
            }
            else
            {
                response.Error.Add(new Error("Forbidden", "You are not allowed here naive."));
            }
            return Json(response);
        }
        [HttpGet]
        public async Task<JsonResult> GetAssignmentById(int assignmentID)
        {
            JsonResponse<StudentAssignment> response = new JsonResponse<StudentAssignment>();
            StudentAssignment studentAssignment = studentAssignmentRepo.FindByID(assignmentID);
            CourseGroup courseGroup = await GetCourseGroup(studentAssignment.CourseAssignment.FK_COURSE.ID);
            AppUser user = await userManager.GetUserAsync(HttpContext.User);

            if (this.isAdmin || this.isInstructor && studentAssignment.CourseAssignment.FK_COURSE.FK_INSTRUCTOR.Id == user.Id)
            {
                if (studentAssignment != null)
                {
                    response.Data.Add(studentAssignment);
                }
            }
            else if (this.isStudent)
            {
                if (courseGroup != null)
                {
                    if (studentAssignment != null)
                    {
                        if (studentAssignment.AppUser.Id == user.Id)
                        {
                            response.Data.Add(studentAssignment);
                        }
                        else
                        {
                            response.Error.Add(new Error("Forbidden", "This is not your assignment."));
                        }
                    }
                    else
                    {
                        response.Error.Add(new Error("NotFound", "The Assignment was not found."));
                    }
                }
                else
                {
                    response.Error.Add(new Error("NotFound", "You aren't or weren't enrolled in this course."));
                }
            }
            else
            {
                response.Error.Add(new Error("Forbidden", "You are not allowed here naive."));
            }
            return Json(response); 
        }

        [HttpGet]
        public async Task<JsonResult> GetAssignmentsByCourseAndReviewGroup(int courseId, string reviewGroupId)
        {
            JsonResponse<StudentAssignment> response = new JsonResponse<StudentAssignment>();
            AppUser user = await userManager.GetUserAsync(HttpContext.User);
            List<StudentAssignment> studentAssignments = studentAssignmentRepo.GetAll();
            Course course = courseRepo.FindByID(courseId);
            CourseGroup courseGroup = await GetCourseGroup(courseId);

            if(this.isAdmin || this.isInstructor && course.FK_INSTRUCTOR == user)
            {
                foreach (StudentAssignment sa in studentAssignments)
                {
                    if (sa.CourseAssignment.FK_COURSE == course && courseGroup.ReviewGroup == reviewGroupId)
                    {
                        response.Data.Add(sa);
                    }
                }
                if (response.Data.Count == 0)
                    response.Error.Add(new Error("No Assignments", "No Assignments"));
            }
            else if(this.isStudent)
            {
                foreach (StudentAssignment sa in studentAssignments)
                {
                    if (sa.CourseAssignment.FK_COURSE == course && courseGroup.ReviewGroup == reviewGroupId)
                    {
                        response.Data.Add(sa);
                    }
                }
                if (response.Data.Count == 0)
                    response.Error.Add(new Error("No Assignments", "No Assignments"));
            }
            return Json(response);
        }

        [HttpGet]
        public async Task<JsonResult> GetAssignmentReviewers(int assignmentID)
        {
            JsonResponse<AppUser>response = new JsonResponse<AppUser>();
            StudentAssignment studentAssignment = studentAssignmentRepo.FindByID(assignmentID);
            List<Review> reviews = reviewRepo.GetAll();
            List<AppUser> reviewers = new List<AppUser>();
            CourseGroup course = await GetCourseGroup(studentAssignment.CourseAssignment.FK_COURSE.ID);
            AppUser user = await userManager.GetUserAsync(HttpContext.User);

            if (studentAssignment != null)
            {
                if (this.isAdmin || this.isInstructor && studentAssignment.CourseAssignment.FK_COURSE.FK_INSTRUCTOR.Id == user.Id)
                {
                    AppUser reviewer = null;
                    foreach(Review r in reviews)
                    {
                        if(r.FK_STUDENT_ASSIGNMENT == studentAssignment)
                        {
                            reviewer = await userManager.FindByIdAsync(studentAssignment.AppUser.Id);
                            response.Data.Add(reviewer);
                        }
                    }
                    if (response.Data.Count == 0)
                        response.Error.Add(new Error("No Assignments", "No Assignments"));
                }
                else if (this.isStudent)
                {
                    if (course != null)
                    {
                        AppUser reviewer = null;
                        foreach (Review r in reviews)
                        {
                            if (r.FK_STUDENT_ASSIGNMENT == studentAssignment)
                            {
                                reviewer = await userManager.FindByIdAsync(studentAssignment.AppUser.Id);
                                response.Data.Add(reviewer);
                            }
                        }
                        if (response.Data.Count == 0)
                            response.Error.Add(new Error("No Assignments", "No Assignments"));
                    }
                    else
                    {
                        response.Error.Add(new Error("NotFound", "You aren't or weren't enrolled in this course."));
                    }
                }
                else
                {
                    response.Error.Add(new Error("Forbidden", "You are not allowed here naive."));
                }
            }
            else
            {
                response.Error.Add(new Error("NotFound", "StudentAssignment was not Found."));
            }
            return Json(response);
        }
        public async Task<JsonResult> CreateStudentAssignment(PFile pFile, CourseAssignment courseAssignment)
        {
            JsonResponse<StudentAssignment> response = new JsonResponse<StudentAssignment>();
            AppUser user = await userManager.GetUserAsync(HttpContext.User);
            if(courseAssignment != null)
            {
                StudentAssignment newAssignment = new StudentAssignment()
                {
                    AppUser = user,
                    FK_PFile = pFile,
                    CourseAssignment = courseAssignment,
                    TimestampCreated = System.DateTime.Now
                };

                if (studentAssignmentRepo.Add(newAssignment) != null)
                {
                    response.Data.Add(newAssignment);
                }
                else
                    response.Error.Add(new Error("Error adding assignment", "Error adding assignment"));
            }
            else
                response.Error.Add(new Error("Invalid CourseId","Ivalid course id"));

            return Json(response);
        }
        [HttpPost]
        public async void UploadStudentAssignment(List<IFormFile> files, CourseAssignment courseAssignment)
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
                if(courseAssignment != null)
                await CreateStudentAssignment(newPFile,courseAssignment);
            }
        }

            #endregion Methods that return Json

            #region StudentAssignmentController Helper Methods

            public async Task<CourseGroup> GetCourseGroup(int courseId)
        {
            AppUser user = await userManager.GetUserAsync(HttpContext.User);
            List<CourseGroup> courseGroups = courseGroupRepo.GetAll();
            CourseGroup studentCourseGroup = null;

            foreach (CourseGroup cg in courseGroups)
            {
                if (cg.FK_Course.ID == courseId && cg.FK_AppUser == user)
                {
                    studentCourseGroup = cg;
                }
            }
            return studentCourseGroup;
        }
        #endregion
    }
}
