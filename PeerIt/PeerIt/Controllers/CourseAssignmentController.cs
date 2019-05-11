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

namespace PeerIt.Controllers
{
    /// <summary>
    /// A controller object that handles all requests to the server regarding Course Assignments.
    /// </summary>
    public class CourseAssignmentController : Controller
    {
        #region Private Variables

        private CourseAssignmentRepository courseAssignmentRepository;
        private CourseGroupRepository courseGroupRepository;
        private CourseRepository courseRepository;
        private UserManager<AppUser> userManager;

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
        public CourseAssignmentController(CourseAssignmentRepository courseAssignmentRepo,
                                          CourseGroupRepository courseGroupRepo,
                                          CourseRepository courseRepo,
                                          UserManager<AppUser> userMgr)
        {
            this.isAdmin = HttpContext.User.IsInRole("Administrator");
            this.isInstructor = HttpContext.User.IsInRole("Instructor");
            this.isStudent = HttpContext.User.IsInRole("Student");

            courseAssignmentRepository = courseAssignmentRepo;
            courseGroupRepository = courseGroupRepo;
            courseRepository = courseRepo;
            userManager = userMgr;
        }

        #endregion Constructors

        #region Methods that return Json

        /// <summary>
        /// Creates an Assignment, and returns if successful.
        /// </summary>
        /// <param name="courseID"></param>
        /// <param name="name"></param>
        /// <param name="dueDate"></param>
        /// <param name="instructions"></param>
        /// <param name="instructionUrl"></param>
        /// <param name="rubric"></param>
        /// <param name="rubricUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> CreateAssignment(int courseID, string name, 
                                           DateTime dueDate, 
                                           string instructions, 
                                           string instructionUrl,
                                           string rubric,
                                           string rubricUrl)
        {
            JsonResponse<CourseAssignment> response = new JsonResponse<CourseAssignment>();
            AppUser user = await userManager.GetUserAsync(HttpContext.User);
            Course course = courseRepository.FindByID(courseID);

            if (course != null)
            {
                if (this.isAdmin || this.isInstructor && course.FK_INSTRUCTOR.Id == user.Id)
                {
                    if (ModelState.IsValid)
                    {
                        CourseAssignment newAssignment = new CourseAssignment
                        {
                            Name = name,
                            FK_COURSE = course,
                            InstructionText = instructions,
                            InstructionsUrl = instructionUrl,
                            RubricText = rubric,
                            RubricUrl = rubricUrl
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
            JsonResponse<List<CourseAssignment>> response = new JsonResponse<List<CourseAssignment>>();
            Course course = courseRepository.FindByID(courseID);
            AppUser user = await userManager.GetUserAsync(HttpContext.User);

            if (course != null)
            {
                if (this.isAdmin)
                {
                    response.Data.Add(courseAssignmentRepository.GetByCourseID(courseID));
                    return Json(response);
                }
                else if (this.isInstructor && course.FK_INSTRUCTOR == user)
                {
                    response.Data.Add(courseAssignmentRepository.GetByCourseID(courseID));
                    return Json(response);
                }
                else if (this.isStudent)
                {
                    CourseGroup courseGroup = courseGroupRepository.GetByUserAndCourseID(user.Id, courseID);
                    if (courseGroup != null)
                    {
                        var responseStudent = new JsonResponse<CourseAssignmentDataOut>();
                        List<CourseAssignment> cAssigns = courseAssignmentRepository.GetByCourseID(courseID);
                        cAssigns.ForEach(cAssign =>
                        {
                            responseStudent.Data.Add(
                                new CourseAssignmentDataOut
                                {
                                    ID = cAssign.ID,
                                    Name = cAssign.Name,
                                    FK_COURSE_ID = cAssign.ID,
                                    InstructionText = cAssign.InstructionText,
                                    InstructionsUrl = cAssign.InstructionsUrl,
                                    RubricText = cAssign.RubricText,
                                    RubricUrl = cAssign.RubricUrl
                                }
                            );
                        });
                        return Json(responseStudent);
                    }
                    else
                    {
                        response.Error.Add(new Error("Forbidden", "You are not a student of this Course."));
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

        /// <summary>
        /// Returns a CourseAssignment by Assignment ID, using a Course ID to
        /// validate.
        /// </summary>
        /// <param name="courseID"></param>
        /// <param name="assignmentID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> Assignment(int courseID, int assignmentID)
        {
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
                    var responseStudent = new JsonResponse<CourseAssignmentDataOut>();
                    CourseGroup sCourse = courseGroupRepository.GetByUserAndCourseID(user.Id, courseID);
                    if (sCourse != null)
                    {
                        CourseAssignment courseAssignment = courseAssignmentRepository.FindByID(assignmentID);

                        if (courseAssignment != null)
                        {
                            responseStudent.Data.Add(
                                new CourseAssignmentDataOut
                                {
                                    ID = courseAssignment.ID,
                                    Name = courseAssignment.Name,
                                    FK_COURSE_ID = courseAssignment.FK_COURSE.ID,
                                    InstructionText = courseAssignment.InstructionText,
                                    InstructionsUrl = courseAssignment.InstructionsUrl,
                                    RubricText = courseAssignment.RubricText,
                                    RubricUrl = courseAssignment.RubricUrl
                                }    
                            );
                            return Json(response);
                        }
                        else
                        {
                            response.Error.Add(new Error("NotFound", "Assignment was not Found."));
                        }
                    }
                    else
                    {
                        response.Error.Add(new Error("Forbidden", "You aren't or weren't enrolled in this course."));
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

        /// <summary>
        /// Sets the instructionText property of a CourseAssignment.
        /// </summary>
        /// <param name="courseID"></param>
        /// <param name="assignmentID"></param>
        /// <param name="instructions"></param>
        /// <returns></returns>
        [HttpPatch]
        public async Task<JsonResult> SetAssignmentInstructions(int courseID, int assignmentID, string instructions)
        {
            JsonResponse<bool> response = new JsonResponse<bool>();
            Course course = courseRepository.FindByID(courseID);
            AppUser user = await userManager.GetUserAsync(HttpContext.User);

            if (this.isAdmin || this.isInstructor && course.FK_INSTRUCTOR.Id == user.Id)
            {
                if (course != null)
                {
                    CourseAssignment assignment = courseAssignmentRepository.FindByID(assignmentID);

                    if (assignment != null)
                    {
                        assignment.InstructionText = instructions;
                        if (courseAssignmentRepository.Edit(assignment))
                        {
                            return Json(response);
                        }
                    }
                    else
                    {
                        response.Error.Add(new Error("NotFound", "Assignment was not Found."));
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

        /// <summary>
        /// Sets the instructionsUrl property of a CourseAssignment.
        /// </summary>
        /// <param name="courseID"></param>
        /// <param name="assignmentID"></param>
        /// <param name="instructionUrl"></param>
        /// <returns></returns>
        [HttpPatch]
        public async Task<JsonResult> SetAssignmentInstructionUrl(int courseID, int assignmentID, string instructionUrl)
        {
            JsonResponse<bool> response = new JsonResponse<bool>();
            Course course = courseRepository.FindByID(courseID);
            AppUser user = await userManager.GetUserAsync(HttpContext.User);

            if (this.isAdmin || this.isInstructor && course.FK_INSTRUCTOR.Id == user.Id)
            {
                if (course != null)
                {
                    CourseAssignment assignment = courseAssignmentRepository.FindByID(assignmentID);

                    if (assignment != null)
                    {
                        assignment.InstructionsUrl = instructionUrl;
                        if (courseAssignmentRepository.Edit(assignment))
                        {
                            return Json(response);
                        }
                    }
                    else
                    {
                        response.Error.Add(new Error("NotFound", "Assignment was not Found."));
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

        /// <summary>
        /// Sets the RubricText property of a CourseAssignment.
        /// </summary>
        /// <param name="courseID"></param>
        /// <param name="assignmentID"></param>
        /// <param name="rubric"></param>
        /// <returns></returns>
        [HttpPatch]
        public async Task<JsonResult> SetAssignmentRubric(int courseID, int assignmentID, string rubric)
        {
            JsonResponse<bool> response = new JsonResponse<bool>();
            Course course = courseRepository.FindByID(courseID);
            AppUser user = await userManager.GetUserAsync(HttpContext.User);

            if (this.isAdmin || this.isInstructor && course.FK_INSTRUCTOR.Id == user.Id)
            {
                if (course != null)
                {
                    CourseAssignment assignment = courseAssignmentRepository.FindByID(assignmentID);

                    if (assignment != null)
                    {
                        assignment.RubricText = rubric;
                        if (courseAssignmentRepository.Edit(assignment))
                        {
                            return Json(response);
                        }
                        else
                        {
                            response.Error.Add(new Error("NotSuccessful", " The data was not successfully written."));
                        }
                    }
                    else
                    {
                        response.Error.Add(new Error("NotFound", "Assignment was not Found."));
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

        /// <summary>
        /// Sets the RubricUrl property of a CourseAssignment.
        /// </summary>
        /// <param name="courseID"></param>
        /// <param name="assignmentID"></param>
        /// <param name="rubricUrl"></param>
        /// <returns></returns>
        [HttpPatch]
        public async Task<JsonResult> SetAssignmentRubricUrl(int courseID, int assignmentID, string rubricUrl)
        {
            JsonResponse<bool> response = new JsonResponse<bool>();
            Course course = courseRepository.FindByID(courseID);
            AppUser user = await userManager.GetUserAsync(HttpContext.User);

            if (this.isAdmin || this.isInstructor && course.FK_INSTRUCTOR.Id == user.Id)
            {
                if (course != null)
                {
                    CourseAssignment assignment = courseAssignmentRepository.FindByID(assignmentID);

                    if (assignment != null)
                    {
                        assignment.RubricUrl = rubricUrl;
                        if (courseAssignmentRepository.Edit(assignment))
                        {
                            return Json(response);
                        }
                        else
                        {
                            response.Error.Add(new Error("NotSuccessful", "The data was not successfully written."));
                        }
                    }
                    else
                    {
                        response.Error.Add(new Error("NotFound", "Assignment was not Found."));
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

        #endregion Methods that return Json
    }
}
