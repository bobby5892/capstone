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
    /// A Controller object that Handles all requests to the server regarding Student Assignments.
    /// </summary>
    public class StudentAssignmentController : Controller
    {
        #region Private Variables

        private CourseRepository courseRepository;
        private StudentAssignmentRepository sAssignmentRepository;
        private CourseAssignmentRepository cAssignmentRepository;
        private CourseGroupRepository cGroupRepository;
        private ActiveReviewerRepository aReviewerRepository;
        private UserManager<AppUser> userManager;

        private bool isAdmin;
        private bool isInstructor;
        private bool isStudent;

        #endregion Private Variables

        #region Constructors

        /// <summary>
        /// Overloaded Constructor
        /// </summary>
        /// <param name="sAssignRepo"></param>
        /// <param name="courseRepo"></param>
        /// <param name="cAssignRepo"></param>
        /// <param name="cGroupRepo"></param>
        /// <param name="aReviewerRepo"></param>
        /// <param name="userMgr"></param>
        public StudentAssignmentController(StudentAssignmentRepository sAssignRepo,
                                           CourseRepository courseRepo,
                                           CourseAssignmentRepository cAssignRepo,
                                           CourseGroupRepository cGroupRepo,
                                           ActiveReviewerRepository aReviewerRepo,
                                           UserManager<AppUser> userMgr)
        {
            sAssignmentRepository = sAssignRepo;
            courseRepository = courseRepo;
            cAssignmentRepository = cAssignRepo;
            cGroupRepository = cGroupRepo;
            aReviewerRepository = aReviewerRepo;
            userManager = userMgr;

            this.isAdmin = HttpContext.User.IsInRole("Administrator");
            this.isInstructor = HttpContext.User.IsInRole("Instructor");
            this.isStudent = HttpContext.User.IsInRole("Student");
        }

        #endregion Constructors

        #region Methods that return Json

        /// <summary>
        /// Returns a list of StudentAssignments by Course ID.
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetAssignmentsByCourse(int courseID)
        {
            JsonResponse<List<StudentAssignment>> response = new JsonResponse<List<StudentAssignment>>();
            Course course = courseRepository.FindByID(courseID);
            AppUser user = await userManager.GetUserAsync(HttpContext.User);

            if (this.isAdmin || this.isInstructor && course.FK_INSTRUCTOR == user)
            {
                if (course != null)
                {
                    List<StudentAssignment> sAssignments = sAssignmentRepository.GetByCourseID(courseID);
                    response.Data.Add(sAssignments);
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
                    CourseGroup sCourse = cGroupRepository.GetByUserAndCourseID(user.Id, courseID);
                    if (sCourse != null)
                    {
                        List<StudentAssignment> sAssignments = sAssignmentRepository.GetByStudentAndCourseID(user.Id, courseID);
                        response.Data.Add(sAssignments);
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

        /// <summary>
        /// Returns a list of StudentAssignments that are ungraded by Course ID.
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetAssignmentsByCourseUngraded(int courseID)
        {
            JsonResponse<List<StudentAssignment>> response = new JsonResponse<List<StudentAssignment>>();
            Course course = courseRepository.FindByID(courseID);
            AppUser user = await userManager.GetUserAsync(HttpContext.User);

            if (this.isAdmin || this.isInstructor && course.FK_INSTRUCTOR == user)
            {
                if (course != null)
                {
                    List<StudentAssignment> sAssignments = sAssignmentRepository.GetByCourseIDUngraded(courseID);
                    response.Data.Add(sAssignments);
                }
                else
                {
                    response.Error.Add(new Error("NotFound", "Course was not Found."));
                }
                return Json(response);
            }
            else if (this.isStudent)
            {
                CourseGroup sCourse = cGroupRepository.GetByUserAndCourseID(user.Id, courseID);
                if (sCourse != null)
                {
                    List<StudentAssignment> sAssignments = sAssignmentRepository.GetByStudentAndCourseIDUngraded(user.Id, courseID);
                    response.Data.Add(sAssignments);
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

        /// <summary>
        /// Returns a list of StudentAssignments by a student's ID and a
        /// student's course ID
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="courseID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetAssignmentsByUser(string userID, int courseID)
        {
            JsonResponse<List<StudentAssignment>> response = new JsonResponse<List<StudentAssignment>>();
            Course course = courseRepository.FindByID(courseID);
            AppUser user = await userManager.FindByIdAsync(userID);

            if (this.isAdmin || this.isInstructor && course.FK_INSTRUCTOR.Id == user.Id)
            {
                if (course != null)
                {
                    if (user != null)
                    {
                        response.Data.Add(sAssignmentRepository.GetByStudentAndCourseID(userID, courseID));
                    }
                    else
                    {
                        response.Error.Add(new Error("NotFound", "User was not Found."));
                    }
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
                    if (user != null)
                    {
                        if (user.Id == userID)
                        {
                            response.Data.Add(sAssignmentRepository.GetByStudentAndCourseID(userID, courseID));
                        }
                        else
                        {
                            response.Error.Add(new Error("Forbidden", "Not your Assignments"));
                        }
                    }
                    else
                    {
                        response.Error.Add(new Error("NotFound", "User was not Found."));
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
        /// Returns a StudentAssignment by it's ID.
        /// </summary>
        /// <param name="assignmentID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetAssignment(int assignmentID)
        {
            JsonResponse<StudentAssignment> response = new JsonResponse<StudentAssignment>();
            StudentAssignment sAssign = sAssignmentRepository.FindByID(assignmentID);
            AppUser user = await userManager.GetUserAsync(HttpContext.User);

            if (this.isAdmin || this.isInstructor && sAssign.CourseAssignment.FK_COURSE.FK_INSTRUCTOR.Id == user.Id)
            {
                if (sAssign != null)
                {
                    response.Data.Add(sAssign);
                }
            }
            else if (this.isStudent)
            {
                CourseGroup sCourse = cGroupRepository.GetByUserAndCourseID(user.Id, sAssign.CourseAssignment.FK_COURSE.ID);
                if (sCourse != null)
                {
                    if (sAssign != null)
                    {
                        if (sAssign.AppUser.Id == user.Id)
                        {
                            response.Data.Add(sAssign);
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

        /// <summary>
        /// Returns a List of StudentAssignments by the Review Group ID.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetAssignmentsByGroup(int groupID)
        {
            JsonResponse<StudentAssignment> response = new JsonResponse<StudentAssignment>();
            response.Error.Add(new Error("NotImplemented", "Feature has not been implemented yet."));
            return Json(response);
        }

        /// <summary>
        /// Returns the ActiveReviewers of a StudentAssignment by the Assignment ID.
        /// </summary>
        /// <param name="assignmentID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetAssignmentReviewers(int assignmentID)
        {
            JsonResponse<List<ActiveReviewer>> response = new JsonResponse<List<ActiveReviewer>>();
            StudentAssignment sAssign = sAssignmentRepository.FindByID(assignmentID);
            AppUser user = await userManager.GetUserAsync(HttpContext.User);

            if (sAssign != null)
            {
                if (this.isAdmin || this.isInstructor && sAssign.CourseAssignment.FK_COURSE.FK_INSTRUCTOR.Id == user.Id)
                {
                    response.Data.Add(aReviewerRepository.GetByStudentAssignmentID(assignmentID));
                }
                else if (this.isStudent)
                {
                    CourseGroup sCourse = cGroupRepository.GetByUserAndCourseID(user.Id, sAssign.CourseAssignment.FK_COURSE.ID);
                    if (sCourse != null)
                    {
                        if (sAssign.AppUser.Id == user.Id)
                        {
                            response.Data.Add(aReviewerRepository.GetByStudentAssignmentID(assignmentID));
                        }
                        else
                        {
                            response.Error.Add(new Error("Forbidden", "This is not your Assignment."));
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
            }
            else
            {
                response.Error.Add(new Error("NotFound", "StudentAssignment was not Found."));
            }
            return Json(response);
        }

        /// <summary>
        /// Sets the Content Property of a StudentAssignment.
        /// </summary>
        /// <param name="assignmentID"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [HttpPatch]
        public async Task<JsonResult> SetContent(int assignmentID, string content)
        {
            JsonResponse<bool> response = new JsonResponse<bool>();
            StudentAssignment sAssign = sAssignmentRepository.FindByID(assignmentID);
            AppUser user = await userManager.GetUserAsync(HttpContext.User);

            if (sAssign != null)
            {
                if (this.isAdmin || this.isInstructor && sAssign.CourseAssignment.FK_COURSE.FK_INSTRUCTOR.Id == user.Id)
                {
                    sAssign.Content = content;
                    if (sAssignmentRepository.Edit(sAssign))
                    {
                        return Json(response);
                    }
                    else
                    {
                        response.Error.Add(new Error("NotSuccessful", "The data was not successfully written."));
                    }
                }
                else if (this.isStudent)
                {
                    CourseGroup sCourse = cGroupRepository.GetByUserAndCourseID(user.Id, sAssign.CourseAssignment.FK_COURSE.ID);
                    if (sCourse != null)
                    {
                        if (sAssign.AppUser.Id == user.Id)
                        {
                            sAssign.Content = content;
                            if (sAssignmentRepository.Edit(sAssign))
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
                            response.Error.Add(new Error("Forbidden", "This is not your Assignment."));
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
                response.Error.Add(new Error("NotFound", "StudentAssignment was not Found."));
            }
            return Json(response);
        }

        /// <summary>
        /// Sets the Score property of a StudentAssignment.
        /// </summary>
        /// <param name="assignmentID"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        [HttpPatch]
        public async Task<JsonResult> SetScore(int assignmentID, int score)
        {
            JsonResponse<bool> response = new JsonResponse<bool>();
            StudentAssignment sAssign = sAssignmentRepository.FindByID(assignmentID);
            AppUser user = await userManager.GetUserAsync(HttpContext.User);

            if (sAssign != null)
            {
                if (this.isAdmin || this.isInstructor && sAssign.CourseAssignment.FK_COURSE.FK_INSTRUCTOR.Id == user.Id)
                {
                    sAssign.Score = score;
                    if (sAssignmentRepository.Edit(sAssign))
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
                    response.Error.Add(new Error("Forbidden", "You are not allowed here naive."));
                }
            }
            else
            {
                response.Error.Add(new Error("NotFound", "StudentAssignment was not Found."));
            }
            return Json(response);
        }

        /// <summary>
        /// Sets the Status property of a StudentAssignment.
        /// </summary>
        /// <param name="assignmentID"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPatch]
        public async Task<JsonResult> SetStatus(int assignmentID, string status)
        {
            JsonResponse<bool> response = new JsonResponse<bool>();
            StudentAssignment sAssign = sAssignmentRepository.FindByID(assignmentID);
            AppUser user = await userManager.GetUserAsync(HttpContext.User);

            if (sAssign != null)
            {
                if (this.isAdmin || this.isInstructor && sAssign.CourseAssignment.FK_COURSE.FK_INSTRUCTOR.Id == user.Id)
                {
                    sAssign.Status = status;
                    if (sAssignmentRepository.Edit(sAssign))
                    {
                        return Json(response);
                    }
                }
                // Need to add ReviewGroups so students who review other student's assignments can set the status property.
                else if (this.isStudent)
                {
                    CourseGroup sCourse = cGroupRepository.GetByUserAndCourseID(user.Id, sAssign.CourseAssignment.FK_COURSE.ID);
                    if (sCourse != null)
                    {
                        if (sAssign.AppUser.Id == user.Id)
                        {
                            sAssign.Status = status;
                            if (sAssignmentRepository.Edit(sAssign))
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
                            response.Error.Add(new Error("Forbidden", "This is not your assignment."));
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
                response.Error.Add(new Error("NotFound", "StudentAssignment was not Found."));
            }
            return Json(response);
        }

        /// <summary>
        /// Deletes an Assignment by it's ID.
        /// </summary>
        /// <param name="assignmentID"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<JsonResult> DeleteAssignment(int assignmentID)
        {
            JsonResponse<bool> response = new JsonResponse<bool>();
            StudentAssignment sAssign = sAssignmentRepository.FindByID(assignmentID);
            AppUser user = await userManager.GetUserAsync(HttpContext.User);

            if (sAssign != null)
            {
                if (this.isAdmin || this.isInstructor && sAssign.CourseAssignment.FK_COURSE.FK_INSTRUCTOR.Id == user.Id)
                {
                    if (sAssignmentRepository.Delete(sAssign))
                    {
                        return Json(response);
                    }
                    else
                    {
                        response.Error.Add(new Error("NotSuccessful", "The data was not successfully written."));
                    }
                }
                else if (this.isStudent)
                {
                    if (sAssign.AppUser.Id == user.Id)
                    {
                        if (sAssignmentRepository.Delete(sAssign))
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
                        response.Error.Add(new Error("Forbidden", "This is not your Assignment"));
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
        
        #endregion Methods that return Json
    }
}
