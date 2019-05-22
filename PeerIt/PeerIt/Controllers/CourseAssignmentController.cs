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
using System.IO;
using Microsoft.AspNetCore.Http;

namespace PeerIt.Controllers
{
    
    /// <summary>
    /// A controller object that handles all requests to the server regarding Course Assignments.
    /// </summary>
    public class CourseAssignmentController : Controller
    {
        #region Private Variables

        private IGenericRepository<CourseAssignment,int> courseAssignmentRepository;
        private IGenericRepository<CourseGroup,int> courseGroupRepository;
        private IGenericRepository<Course,int> courseRepository;
        private UserManager<AppUser> userManager;
        private IGenericRepository<PFile, string> pFileRepo;
        private List<PFile> pFiles;

        private bool isAdmin;
        private bool isInstructor;
        private bool isStudent;
        private AppUser currentUser;
        private List<CourseGroup> courseGroups;

        #endregion Private Variables

        #region Constructors

        /// <summary>
        /// Overloaded Constructor
        /// </summary>
        /// <param name="courseAssignmentRepo"></param>
        /// <param name="courseGroupRepo"></param>
        /// <param name="courseRepo"></param>
        /// <param name="userMgr"></param>
        /// <param name="pFileRepository"></param>
        public CourseAssignmentController(IGenericRepository<CourseAssignment,int> courseAssignmentRepo,
                                          IGenericRepository<CourseGroup,int> courseGroupRepo,
                                          IGenericRepository<Course,int> courseRepo,
                                          UserManager<AppUser> userMgr,
                                          IGenericRepository<PFile, string> pFileRepository)
        {
            courseAssignmentRepository = courseAssignmentRepo;
            courseGroupRepository = courseGroupRepo;
            courseRepository = courseRepo;
            userManager = userMgr;
            pFileRepo = pFileRepository;
        }

        #endregion Constructors
        private void SetRoles()
        {
            this.isAdmin = HttpContext.User.IsInRole("Administrator");
            this.isInstructor = HttpContext.User.IsInRole("Instructor");
            this.isStudent = HttpContext.User.IsInRole("Student");
        }

        #region Methods that return Json

        /// <summary>
        /// Creates an Assignment, and returns if successful.
        /// </summary>
        /// <param name="courseID"></param>
        /// <param name="assignmentname"></param>
        /// <param name="dueDate"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> CreateAssignment(int courseID, string assignmentname, List<IFormFile> files,
                                           string dueDate)
        {
            SetRoles();
         //   DateTime.ParseExact(dueDate,"mm/DD/YYYY");
            JsonResponse<CourseAssignment> response = new JsonResponse<CourseAssignment>();
            AppUser user = await userManager.GetUserAsync(HttpContext.User);
            Course course = courseRepository.FindByID(courseID);
            PFile newPFile = new PFile();
            if (course != null)
            {
                Stream stream;
                Guid guidFileId;
                long size = files.Sum(f => f.Length);
                foreach (var formFile in files)
                {
                    guidFileId = Guid.NewGuid();
                    string ext = formFile.FileName.Split(".")[1];
                    string filename = formFile.FileName.Split(".")[0];
                    //AppUser user = await userManager.GetUserAsync(HttpContext.User);

                    string destinationFolder = "Data/" + guidFileId + "." + ext;


                    if (formFile.Length > 0)
                    {
                        using (stream = new FileStream(destinationFolder, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                    }
                    newPFile = new PFile(guidFileId.ToString(), filename, ext, user);
                    pFileRepo.Add(newPFile);
                    pFiles = pFileRepo.GetAll();
                }
                if (this.isAdmin || this.isInstructor && course.FK_INSTRUCTOR.Id == user.Id)
                {
                    if (ModelState.IsValid)
                    {
                        CourseAssignment newAssignment = new CourseAssignment
                        {
                            Name = assignmentname,
                            FK_COURSE = course,
                            PFile = newPFile,
                            DueDate = DateTime.Parse(dueDate)
                        };

                        if (courseAssignmentRepository.Add(newAssignment) != null)
                        {
                            response.Data.Add(newAssignment);
                            return Json(response);
                        }
                        else
                        {
                            response.Error.Add(new Error("NotSuccessful", "The data was not successfully written."));
                        }
                    }
                }
                else
                {
                    response.Error.Add(new Error("Forbidden", "You are not allowed here naive."));
                }
            }
            else
            {
                response.Error.Add(new Error("NotFound", "The course was not found."));
            }
            return Json(response);

        }

        /// <summary>
        /// Returns a List of CourseAssignments by a Course's ID
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> Assignments(int courseID)
        {
            SetRoles();
            JsonResponse<CourseAssignment> response = new JsonResponse<CourseAssignment>();
            Course course = courseRepository.FindByID(courseID);
            AppUser user = await userManager.GetUserAsync(HttpContext.User);
            if (course != null)
            {
                if (this.isAdmin)
                {
                   // response.Data.Add(courseAssignmentRepository.GetByCourseID(courseID));
                    response.Data = courseAssignmentRepository.GetAll().FindAll((x) => {
                           if(x.FK_COURSE.ID == courseID) { return true; }
                        return false;
                    });
                    return Json(response);
                }
                else if (this.isInstructor && course.FK_INSTRUCTOR == user)
                {
                    response.Data = courseAssignmentRepository.GetAll().FindAll((x) => {
                        if (x.FK_COURSE.ID == courseID) { return true; }
                        return false;
                    });
                    return Json(response);
                }
                else if (this.isStudent)
                {
                    response.Data = courseAssignmentRepository.GetAll().FindAll((x) => {
                        if (x.FK_COURSE.ID == courseID) { return true; }
                        return false;
                    });

                    return Json(response);
                    
                }
                else
                {
                    response.Error.Add(new Error("Forbidden", "You are not allowed here."));
                }
            }
            else
            {
                response.Error.Add(new Error("NotFound", "Course was not Found."));
            }
            return Json(response);
        }
       

        ///// <summary>
        ///// Returns a CourseAssignment by Assignment ID, using a Course ID to
        ///// validate.
        ///// </summary>
        ///// <param name="courseID"></param>
        ///// <param name="assignmentID"></param>
        ///// <returns></returns>
        //[HttpGet]
       public async Task<JsonResult> Assignment(int courseID, int assignmentID)
        {
            SetRoles();
            JsonResponse<CourseAssignment> response = new JsonResponse<CourseAssignment>();
            Course course = courseRepository.FindByID(courseID);
            AppUser user = await userManager.GetUserAsync(HttpContext.User);

            if (course != null)
            {
                if (this.isAdmin || this.isInstructor && course.FK_INSTRUCTOR.Id == user.Id)
                {
                    CourseAssignment courseAssignment = courseAssignmentRepository.FindByID(assignmentID);
        
                    if (courseAssignment != null)
                    {
                        response.Data.Add(courseAssignment);
                        return Json(response);
                    }
                    else
                    {
                        response.Error.Add(new Error("NotFound", "Assignment was not Found."));
                    }
                }
                else if (this.isStudent)
                {
                    CourseAssignment courseAssignment = courseAssignmentRepository.FindByID(assignmentID);

                    if (courseAssignment != null)
                    {
                        response.Data.Add(courseAssignment);
                        return Json(response);
                    }
                    else
                    {
                        response.Error.Add(new Error("NotFound", "Assignment was not Found."));
                    }
                }
                else
                {
                    response.Error.Add(new Error("Forbidden", "You are not allowed here naive."));
                }
            }
            else
            {
                response.Error.Add(new Error("NotFound", "Course was not Found."));
            }
            return Json(response);
        }
        public async Task<JsonResult> DeleteAssignment(int assignmentID)
        {
            JsonResponse<CourseAssignment> response = new JsonResponse<CourseAssignment>();
            SetRoles();
            if(this.isAdmin || this.isInstructor)
            {
                CourseAssignment ca = this.courseAssignmentRepository.FindByID(assignmentID);
                if(ca != null)
                {
                    this.courseAssignmentRepository.Delete(ca);
                }
                else
                {
                    response.Error.Add(new Error() { Description = "No such assignment", Name = "CourseAssignment" });
                }
            }
            else
            {
                response.Error.Add(new Error() { Description = "Your not an instructor or admin", Name = "CourseAssignment" });
            }
            return Json(response);
        }
            /// <summary>
            /// Sets the instructionText property of a CourseAssignment.
            /// </summary>
            /// <param name="courseID"></param>
            /// <param name="assignmentID"></param>
            /// <param name="instructions"></param>
            /// <returns></returns>
            //[HttpPatch]
            //public async Task<JsonResult> SetAssignmentInstructions(int courseID, int assignmentID, string instructions)
            //{
            //    JsonResponse<bool> response = new JsonResponse<bool>();
            //    Course course = courseRepository.FindByID(courseID);
            //    AppUser user = await userManager.GetUserAsync(HttpContext.User);

            //    if (this.isAdmin || this.isInstructor && course.FK_INSTRUCTOR.Id == user.Id)
            //    {
            //        if (course != null)
            //        {
            //            CourseAssignment assignment = courseAssignmentRepository.FindByID(assignmentID);

            //            if (assignment != null)
            //            {
            //                assignment.InstructionText = instructions;
            //                if (courseAssignmentRepository.Edit(assignment))
            //                {
            //                    return Json(response);
            //                }
            //            }
            //            else
            //            {
            //                response.Error.Add(new Error("NotFound", "Assignment was not Found."));
            //            }
            //        }
            //        else
            //        {
            //            response.Error.Add(new Error("NotFound", "Course was not Found."));
            //        }
            //    }
            //    else
            //    {
            //        response.Error.Add(new Error("Forbidden", "You are not allowed here naive."));
            //    }
            //    return Json(response);
            //}

            ///// <summary>
            ///// Sets the instructionsUrl property of a CourseAssignment.
            ///// </summary>
            ///// <param name="courseID"></param>
            ///// <param name="assignmentID"></param>
            ///// <param name="instructionUrl"></param>
            ///// <returns></returns>
            //[HttpPatch]
            //public async Task<JsonResult> SetAssignmentInstructionUrl(int courseID, int assignmentID, string instructionUrl)
            //{
            //    JsonResponse<bool> response = new JsonResponse<bool>();
            //    Course course = courseRepository.FindByID(courseID);
            //    AppUser user = await userManager.GetUserAsync(HttpContext.User);

            //    if (this.isAdmin || this.isInstructor && course.FK_INSTRUCTOR.Id == user.Id)
            //    {
            //        if (course != null)
            //        {
            //            CourseAssignment assignment = courseAssignmentRepository.FindByID(assignmentID);

            //            if (assignment != null)
            //            {
            //                assignment.InstructionsUrl = instructionUrl;
            //                if (courseAssignmentRepository.Edit(assignment))
            //                {
            //                    return Json(response);
            //                }
            //            }
            //            else
            //            {
            //                response.Error.Add(new Error("NotFound", "Assignment was not Found."));
            //            }
            //        }
            //        else
            //        {
            //            response.Error.Add(new Error("NotFound", "Course was not Found."));
            //        }
            //    }
            //    else
            //    {
            //        response.Error.Add(new Error("Forbidden", "You are not allowed here naive."));
            //    }
            //    return Json(response);
            //}

            ///// <summary>
            ///// Sets the RubricText property of a CourseAssignment.
            ///// </summary>
            ///// <param name="courseID"></param>
            ///// <param name="assignmentID"></param>
            ///// <param name="rubric"></param>
            ///// <returns></returns>
            //[HttpPatch]
            //public async Task<JsonResult> SetAssignmentRubric(int courseID, int assignmentID, string rubric)
            //{
            //    JsonResponse<bool> response = new JsonResponse<bool>();
            //    Course course = courseRepository.FindByID(courseID);
            //    AppUser user = await userManager.GetUserAsync(HttpContext.User);

            //    if (this.isAdmin || this.isInstructor && course.FK_INSTRUCTOR.Id == user.Id)
            //    {
            //        if (course != null)
            //        {
            //            CourseAssignment assignment = courseAssignmentRepository.FindByID(assignmentID);

            //            if (assignment != null)
            //            {
            //                assignment.RubricText = rubric;
            //                if (courseAssignmentRepository.Edit(assignment))
            //                {
            //                    return Json(response);
            //                }
            //                else
            //                {
            //                    response.Error.Add(new Error("NotSuccessful", " The data was not successfully written."));
            //                }
            //            }
            //            else
            //            {
            //                response.Error.Add(new Error("NotFound", "Assignment was not Found."));
            //            }
            //        }
            //        else
            //        {
            //            response.Error.Add(new Error("NotFound", "Course was not Found."));
            //        }
            //    }
            //    else
            //    {
            //        response.Error.Add(new Error("Forbidden", "You are not allowed here naive."));
            //    }
            //    return Json(response);
            //}

            ///// <summary>
            ///// Sets the RubricUrl property of a CourseAssignment.
            ///// </summary>
            ///// <param name="courseID"></param>
            ///// <param name="assignmentID"></param>
            ///// <param name="rubricUrl"></param>
            ///// <returns></returns>
            //[HttpPatch]
            //public async Task<JsonResult> SetAssignmentRubricUrl(int courseID, int assignmentID, string rubricUrl)
            //{
            //    JsonResponse<bool> response = new JsonResponse<bool>();
            //    Course course = courseRepository.FindByID(courseID);
            //    AppUser user = await userManager.GetUserAsync(HttpContext.User);

            //    if (this.isAdmin || this.isInstructor && course.FK_INSTRUCTOR.Id == user.Id)
            //    {
            //        if (course != null)
            //        {
            //            CourseAssignment assignment = courseAssignmentRepository.FindByID(assignmentID);

            //            if (assignment != null)
            //            {
            //                assignment.RubricUrl = rubricUrl;
            //                if (courseAssignmentRepository.Edit(assignment))
            //                {
            //                    return Json(response);
            //                }
            //                else
            //                {
            //                    response.Error.Add(new Error("NotSuccessful", "The data was not successfully written."));
            //                }
            //            }
            //            else
            //            {
            //                response.Error.Add(new Error("NotFound", "Assignment was not Found."));
            //            }
            //        }
            //        else
            //        {
            //            response.Error.Add(new Error("NotFound", "Course was not Found."));
            //        }
            //    }
            //    else
            //    {
            //        response.Error.Add(new Error("Forbidden", "You are not allowed here naive."));
            //    }
            //    return Json(response);
            //}

            #endregion Methods that return Json
        }
}
