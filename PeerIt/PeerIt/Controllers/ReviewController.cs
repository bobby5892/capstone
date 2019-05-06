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
namespace PeerIt.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ReviewController : Controller
    {
        #region Private Variables

        private ReviewRepository reviewRepository;
        private StudentAssignmentRepository studentAssignmentRepository;
        private CourseGroupRepository courseGroupRepository;
        //private JsonResponse<Review> response;
        //private List<Review> reviews;
        //private Review review;
        private StudentAssignment studentAssignment;
        private UserManager<AppUser> userManager;

        private bool isAdmin;
        private bool isInstructor;
        private bool isStudent;

        #endregion Private Variables

        #region Constructors

        /// <summary>
        /// Creates a review controller object and passes the user manager and repos
        /// </summary>
        /// <param name="userMgr"></param>
        /// <param name="reviewRepo"></param>
        /// <param name="studentAssignmentRepo"></param>
        public ReviewController(UserManager<AppUser> userMgr, 
                                ReviewRepository reviewRepo,
                                StudentAssignmentRepository studentAssignmentRepo,
                                CourseGroupRepository courseGroupRepo)
        {
            userManager = userMgr;
            reviewRepository = reviewRepo;
            studentAssignmentRepository = studentAssignmentRepo;
            courseGroupRepository = courseGroupRepo;

            this.isAdmin = HttpContext.User.IsInRole("Administrator");
            this.isInstructor = HttpContext.User.IsInRole("Instructor");
            this.isStudent = HttpContext.User.IsInRole("Student");
        }

        #endregion Constructors

        #region Methods That Return Json

        // Currently working on this.
        /// <summary>
        /// Gets all the reviews with the passed assignment id
        /// </summary>
        /// <param name="assignmentId"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetReviewsByAssignmentId(int assignmentId)
        {
            JsonResponse<List<Review>> response = new JsonResponse<List<Review>>();
            studentAssignment = studentAssignmentRepository.FindByID(assignmentId);
            AppUser user = await userManager.GetUserAsync(HttpContext.User);

            if (studentAssignment != null)
            {
                if (this.isAdmin || this.isInstructor && studentAssignment.CourseAssignment.FK_COURSE.FK_INSTRUCTOR.Id == user.Id)
                {
                    List<Review> reviews = reviewRepository.GetReviewsByAssignment(assignmentId);
                    response.Data.Add(reviews);
                    return Json(response);
                }
                else if (this.isStudent)
                {
                    if (studentAssignment.AppUser.Id == user.Id)
                    {
                        List<Review> reviews = reviewRepository.GetReviewsByAssignment(assignmentId);
                        response.Data.Add(reviews);
                        return Json(response);
                    }
                    else
                    {
                        response.Error.Add(new Error("forbidden", "This is not your assignment."));
                    }
                }
                else
                {
                    response.Error.Add(new Error("Forbidden", "You are not allowed here naive."));
                }
            }
            else
            {
                response.Error.Add(new Error("NotFound", "The student assignment was not found"));
            }
            // Add an error to the list (there can be more than one)
            response.Error.Add(new Error() { Name = "No Reviews", Description = "No reviews found for that assignment" });
            return Json(response);
        }
        /// <summary>
        /// Gets the review at the passed id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetReviewById(int id)
        {
            JsonResponse<Review> response = new JsonResponse<Review>();
            Review review = reviewRepository.FindByID(id);
            AppUser user = await userManager.GetUserAsync(HttpContext.User);

            if (review != null)
            {
                if (this.isAdmin || this.isInstructor && review.FK_STUDENT_ASSIGNMENT.CourseAssignment.FK_COURSE.FK_INSTRUCTOR.Id == user.Id)
                {
                    response.Data.Add(review);
                    return Json(response);
                }
                else if (this.isStudent)
                {
                    if (review.FK_APP_USER.Id == user.Id)
                    {
                        response.Data.Add(review);
                        return Json(response);
                    }
                    else if (review.FK_STUDENT_ASSIGNMENT.AppUser.Id == user.Id)
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
        /// <summary>
        /// Creates a new review and adds it to the database
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="studentAssignmentId"></param>
        /// <returns></returns>
        public async Task<JsonResult> CreateReview(string contents, int studentAssignmentId)
        {
            JsonResponse<Review> response = new JsonResponse<Review>();
            //AppUser user = await GetCurrentUserById(userId);
            AppUser user = await userManager.GetUserAsync(HttpContext.User);
            studentAssignment = studentAssignmentRepository.FindByID(studentAssignmentId);
            Course course = studentAssignment.CourseAssignment.FK_COURSE;
            AppUser assignUser = studentAssignment.AppUser;

            if (studentAssignment != null)
            {
                if (this.isAdmin || this.isInstructor && studentAssignment.CourseAssignment.FK_COURSE.FK_INSTRUCTOR.Id == user.Id)
                {
                    Review review = new Review() { Content = contents, FK_APP_USER = user, FK_STUDENT_ASSIGNMENT = studentAssignment };
                    Review newReview = reviewRepository.Add(review);
                    if (newReview != null)
                    {
                        response.Data.Add(newReview);
                    }
                    else
                    {
                        response.Error.Add(new Error("NotSuccessful", "the data was not successfully written."));
                    }
                }
                else if (this.isStudent)
                {
                    CourseGroup courseGroupCurrentUser = courseGroupRepository.GetByUserAndCourseID(user.Id, course.ID);
                    CourseGroup courseGroupAssignmentUser = courseGroupRepository.GetByUserAndCourseID(assignUser.Id, course.ID);
                    if (courseGroupCurrentUser.ReviewGroup == courseGroupCurrentUser.ReviewGroup)
                    {
                        Review review = new Review() { Content = contents, FK_APP_USER = user, FK_STUDENT_ASSIGNMENT = studentAssignment };
                        Review newReview = reviewRepository.Add(review);
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
                        response.Error.Add(new Error("Forbidden", "you are not in this group."));
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
            /*
            if(studentAssignment == null)
            {
                response.Error.Add(new Error() {Name = "No Student Assignment", Description = "No student assignment for that id"});
                return Json(response);
            }
            try
            {
                Review review = new Review() { Content = contents, FK_APP_USER = user, FK_STUDENT_ASSIGNMENT = studentAssignment };
                reviewRepository.Add(review);
                response.Data.Add(review);
            }
            catch
            {
                response.Error.Add(new Error { Name = "Could not Add", Description = "Could not add the new review to database" });
            }
            */
            return Json(response);
        }

        /*
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<AppUser> GetCurrentUserById(string userId)
        {
            AppUser user = await userManager.FindByIdAsync(userId);
            return user;
        }
        */

        #endregion Methods That Return Json
    }
}
