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
        private ReviewRepository reviewRepo;
        private StudentAssignmentRepository studentAssignmentRepo;
        private JsonResponse<Review> response;
        private List<Review> reviews;
        private Review review;
        private StudentAssignment studentAssignment;
        private UserManager<AppUser> userManager;

        /// <summary>
        /// Creates a review controller object and passes the user manager and repos
        /// </summary>
        /// <param name="userMgr"></param>
        /// <param name="repo"></param>
        public ReviewController(UserManager<AppUser> userMgr,ReviewRepository repo)
        {
            userManager = userMgr;
            reviewRepo = repo;
        }
        /// <summary>
        /// Gets all the reviews with the passed assignment id
        /// </summary>
        /// <param name="assignmentId"></param>
        /// <returns></returns>
        public JsonResult GetReviewsByAssignmentId(int assignmentId)
        {
            response = new JsonResponse<Review>();
            reviews = reviewRepo.GetAll();

            foreach (Review r in reviews)
            {
                if (assignmentId == r.FK_STUDENT_ASSIGNMENT.ID)
                {
                    response.Data.Add(r);
                }
            }
            if (response.Success)
            {
                return Json(response);
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
        public JsonResult GetReviewById(int id)
        {
            response = new JsonResponse<Review>();
            review = reviewRepo.FindByID(id);
            response.Data.Add(review);
            if(response.TotalResults < 0)
            {
                return Json(response);
            }
            response.Error.Add(new Error() { Name = "No Review", Description = "No Review for that Id" });
            return Json(response);
        }
        /// <summary>
        /// Creates a new review and adds it to the database
        /// </summary>
        /// <param name="contents"></param>
        /// <param name="userId"></param>
        /// <param name="studentAssignmentId"></param>
        /// <returns></returns>
        public async Task<JsonResult> CreateReview(string contents, string userId, int studentAssignmentId)
        {
            response = new JsonResponse<Review>();
          
            AppUser user = await GetCurrentUserById(userId);
            studentAssignment = studentAssignmentRepo.FindByID(studentAssignmentId);

            if(studentAssignment == null)
            {
                response.Error.Add(new Error() {Name = "No Student Assignment", Description = "No student assignment for that id"});
                return Json(response);
            }
            try
            {
                review = new Review() { Content = contents, FK_APP_USER = user, FK_STUDENT_ASSIGNMENT = studentAssignment };
                reviewRepo.Add(review);
                response.Data.Add(review);
            }
            catch
            {
                response.Error.Add(new Error { Name = "Could not Add", Description = "Could not add the new review to database" });
            }
            return Json(response);
        }

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
    }
}
