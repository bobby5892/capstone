using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PeerIt.Models;
using PeerIt.Repositories;
using PeerIt.ViewModels;

namespace PeerIt.Controllers
{
    /// <summary>
    /// Comment Controller
    /// </summary>
    public class CommentController : Controller
    {

        private CommentRepository commentRepo;
        private JsonResponse<Comment> response;
        private UserManager<AppUser> userManager;
        private AppUser user;
        private List<Comment> comments;

        /// <summary>
        /// Comment Controller Method
        /// </summary>
        /// <param name="userMgr"></param>
        /// <param name="repo"></param>
        public CommentController(UserManager<AppUser> userMgr, CommentRepository repo)
        {
            userManager = userMgr;
            commentRepo = repo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetCommentsByUserId(string userId)
        {
            response = new JsonResponse<Comment>();
            comments = commentRepo.GetAll();
            user = await GetCurrentUserById(userId);
            
            if(user == null)
            {
                response.Error.Add(new Error() { Name = "No User", Description = "No user with that id" });
                return Json(response);
            }

            foreach(Comment c in comments)
            {
                if(c.FK_APP_USER.Id == userId)
                {
                    response.Data.Add(c);
                }
            }
            if(!response.Success)
            {
                response.Error.Add(new Error() { Name = "No Comments", Description = "No comments by user" });
                return Json(response);
            }
            return Json(response);
        }
        public JsonResult GetCommentsByAssignmentId(int studentAssignmentId)
        {
            response = new JsonResponse<Comment>();
            return Json(response);
        }
        public JsonResult GetCommentCountForUser(string userId)
        {
            response = new JsonResponse<Comment>();
            return Json(response);
        }
        public JsonResult GetCommentCountForAssignment(int studentAssignmentId)
        {
            response = new JsonResponse<Comment>();
            return Json(response);
        }
        public JsonResult CreateComment(string userID, int assignment_id, string comment)
        {
            response = new JsonResponse<Comment>();
            return Json(response);
        }
        public JsonResult DeleteComment(int commentId)
        {
            response = new JsonResponse<Comment>();
            return Json(response);
        }
        public async Task<AppUser> GetCurrentUserById(string userId)
        {
            AppUser user = await userManager.FindByIdAsync(userId);
            return user;
        }

    }

}
