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
    public class CourseAssignmentController : Controller
    {
        #region Private Variables

        private CourseAssignmentRepository courseAssignmentRepository;
        private CourseRepository courseRepository;
        private UserManager<AppUser> userManager;

        #endregion Private Variables

        #region Constructors

        public CourseAssignmentController(CourseAssignmentRepository courseAssignmentRepo,
                                          CourseRepository courseRepo,
                                          UserManager<AppUser> userMgr)
        {
            courseAssignmentRepository = courseAssignmentRepo;
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
            JsonResponse<bool> response = new JsonResponse<bool>();

            if (ModelState.IsValid)
            {
                Course course = courseRepository.FindByID(courseID);

                if (course != null && course.FK_INSTRUCTOR == await userManager.GetUserAsync(HttpContext.User))
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
                        return Json(response);
                    }
                }
            }

            response.Error.Add(new Error("CreateFailed", "The Assignment wasn't created."));
            return Json(response);
        }

        /// <summary>
        /// Returns a List of CourseAssignments by a Course's ID
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Assignments(int courseID)
        {
            JsonResponse<List<CourseAssignment>> response = new JsonResponse<List<CourseAssignment>>();

            Course course = courseRepository.FindByID(courseID);
            if (course != null)
            {
                response.Data.Add(courseAssignmentRepository.GetByCourseID(courseID));
                return Json(response);
            }

            response.Error.Add(new Error("NotFound", "Course was not Found."));
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
        public JsonResult Assignment(int courseID, int assignmentID)
        {
            JsonResponse<CourseAssignment> response = new JsonResponse<CourseAssignment>();
            Course course = courseRepository.FindByID(courseID);

            if (course != null)
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
        public JsonResult SetAssignmentInstructions(int courseID, int assignmentID, string instructions)
        {
            JsonResponse<bool> response = new JsonResponse<bool>();
            Course course = courseRepository.FindByID(courseID);

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
        public JsonResult SetAssignmentInstructionUrl(int courseID, int assignmentID, string instructionUrl)
        {
            JsonResponse<bool> response = new JsonResponse<bool>();
            Course course = courseRepository.FindByID(courseID);

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
        public JsonResult SetAssignmentRubric(int courseID, int assignmentID, string rubric)
        {
            JsonResponse<bool> response = new JsonResponse<bool>();
            Course course = courseRepository.FindByID(courseID);

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
        public JsonResult SetAssignmentRubricUrl(int courseID, int assignmentID, string rubricUrl)
        {
            JsonResponse<bool> response = new JsonResponse<bool>();
            Course course = courseRepository.FindByID(courseID);

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
            return Json(response);
        }

        #endregion Methods that return Json
    }
}
