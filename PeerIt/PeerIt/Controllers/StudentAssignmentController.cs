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
    public class StudentAssignmentController : Controller
    {
        #region Private Variables

        private CourseRepository courseRepository;
        private StudentAssignmentRepository sAssignmentRepository;
        private CourseAssignmentRepository cAssignmentRepository;
        private ActiveReviewerRepository aReviewerRepository;
        private UserManager<AppUser> userManager;

        #endregion Private Variables

        #region Constructors

        public StudentAssignmentController(StudentAssignmentRepository sAssignRepo,
                                           CourseRepository courseRepo,
                                           CourseAssignmentRepository cAssignRepo,
                                           ActiveReviewerRepository aReviewerRepo,
                                           UserManager<AppUser> userMgr)
        {
            sAssignmentRepository = sAssignRepo;
            courseRepository = courseRepo;
            cAssignmentRepository = cAssignRepo;
            aReviewerRepository = aReviewerRepo;
            userManager = userMgr;
        }

        #endregion Constructors

        #region Methods that return Json

        /// <summary>
        /// Returns a list of StudentAssignments by Course ID.
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetAssignmentsByCourse(int courseID)
        {
            JsonResponse<List<StudentAssignment>> response = new JsonResponse<List<StudentAssignment>>();
            Course course = courseRepository.FindByID(courseID);

            if (course != null)
            {
                List<StudentAssignment> sAssignments = sAssignmentRepository.GetByCourseID(courseID);
                response.Data.Add(sAssignments);
            }
            else
            {
                response.Error.Add(new Error("NotFound", "Course was not Found."));
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

            if (course != null)
            {
                if (user != null)
                {
                    response.Data.Add(sAssignmentRepository.getByUserInCourse(userID, courseID));
                }
                else
                {
                    response.Error.Add( new Error("NotFound", "User was not Found."));
                }
            }
            else
            {
                response.Error.Add(new Error("NotFound", "Course was not Found."));
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

            if (sAssign != null)
            {
                response.Data.Add(sAssign);
            }
            else
            {
                response.Error.Add(new Error("NotFound", "StudentAssignment was not Found."));
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
        public JsonResult GetAssignmentReviewers(int assignmentID)
        {
            JsonResponse<List<ActiveReviewer>> response = new JsonResponse<List<ActiveReviewer>>();
            StudentAssignment sAssign = sAssignmentRepository.FindByID(assignmentID);
            
            if (sAssign != null)
            {
                response.Data.Add(aReviewerRepository.GetByStudentAssignmentID(assignmentID));
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
        public JsonResult SetContent(int assignmentID, string content)
        {
            JsonResponse<bool> response = new JsonResponse<bool>();
            StudentAssignment sAssign = sAssignmentRepository.FindByID(assignmentID);

            if (sAssign != null)
            {
                sAssign.Content = content;
                if (sAssignmentRepository.Edit(sAssign))
                {
                    return Json(response);
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
        public JsonResult SetScore(int assignmentID, int score)
        {
            JsonResponse<bool> response = new JsonResponse<bool>();
            StudentAssignment sAssign = sAssignmentRepository.FindByID(assignmentID);

            if (sAssign != null)
            {
                sAssign.Score = score;
                if (sAssignmentRepository.Edit(sAssign))
                {
                    return Json(response);
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
        public JsonResult SetStatus(int assignmentID, string status)
        {
            JsonResponse<bool> response = new JsonResponse<bool>();
            StudentAssignment sAssign = sAssignmentRepository.FindByID(assignmentID);

            if (sAssign != null)
            {
                sAssign.Status = status;
                if (sAssignmentRepository.Edit(sAssign))
                {
                    return Json(response);
                }
            }
            else
            {
                response.Error.Add(new Error("NotFound", "StudentAssignment was not Found."));
            }
            return Json(response);
        }

        [HttpDelete]
        public JsonResult DeleteAssignment(int assignmentID)
        {
            JsonResponse<bool> response = new JsonResponse<bool>();
            StudentAssignment sAssign = sAssignmentRepository.FindByID(assignmentID);

            if (sAssign != null)
            {
                if (sAssignmentRepository.Delete(sAssign))
                {
                    return Json(response);
                }
            }
            response.Error.Add(new Error("NotFound", "StudentAssignment was not Found."));
            return Json(response);
        }



        #endregion Methods that return Json
    }
}
