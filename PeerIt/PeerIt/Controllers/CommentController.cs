using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PeerIt.Interfaces;
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

        private IGenericRepository<Comment, int> commentRepo;
        private IGenericRepository<StudentAssignment,int> studentAssignmentRepo;
        private JsonResponse<Comment> response;
        private UserManager<AppUser> userManager;
        private AppUser user;
        private Comment comment;
        private StudentAssignment studentAssignment;
        private List<Comment> comments;

        /// <summary>
        /// Comment Controller Method
        /// </summary>
        /// <param name="userMgr"></param>
        /// <param name="repo"></param>
        public CommentController(UserManager<AppUser> userMgr, IGenericRepository<Comment,int> cRepo, IGenericRepository<StudentAssignment,int> sRepo )
        {
            userManager = userMgr;
            commentRepo = cRepo;
            studentAssignmentRepo = sRepo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetCommentsByUserId()
        {
            response = new JsonResponse<Comment>();
            comments = commentRepo.GetAll();
            user = await userManager.GetUserAsync(HttpContext.User);
            
            if(user == null)
            {
                response.Error.Add(new Error() { Name = "No User", Description = "There is no current user, Pleas login" });
                return Json(response);
            }

            foreach(Comment c in comments)
            {
                if(c.FK_APP_USER.Id == user.Id)
                {
                    response.Data.Add(c);
                }
            }
            if (response.Data.Count == 0)
            {
                response.Error.Add(new Error() { Name = "No Comments", Description = "No comments by current user" });
                return Json(response);
            }
            return Json(response);
        }

        /// <summary>
        /// Get all the assignments for the student assignment id
        /// </summary>
        /// <param name="studentAssignmentId"></param>
        /// <returns></returns>
        public JsonResult GetCommentsByAssignmentId(int studentAssignmentId)
        {
            response = new JsonResponse<Comment>();
            studentAssignment = studentAssignmentRepo.FindByID(studentAssignmentId);
            comments = commentRepo.GetAll();
            if (studentAssignment != null)
            {
                foreach (Comment c in comments)
                {
                    if (c.FK_STUDENT_ASSIGNMENT.ID == studentAssignmentId)
                    {
                        response.Data.Add(c);
                    }
                }
                if (response.Data.Count == 0)
                {
                    response.Error.Add(new Error() { Name = "No Comments", Description = "No comments for the selected assignment" });
                }
            }
            else
                response.Error.Add(new Error("Null Assignment at Id", "assignment at given id does not exits"));
            return Json(response);
        }
        
        /// <summary>
        /// Get all of the comments for the current user
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetCommentCountForUser()
        {
            user = await userManager.GetUserAsync(HttpContext.User);
            response = new JsonResponse<Comment>();
            comments = commentRepo.GetAll();

            if(user == null)
            {
                response.Error.Add(new Error() { Name = "No current user", Description = "No user currently logged in." });
                return Json(response);
            }
            foreach(Comment c in comments)
            {
                if(c.FK_APP_USER == user)
                {
                    response.Data.Add(c);
                }
            }
            if(!response.Success)
            {
                response.Error.Add(new Error() { Name = "No comments", Description = "No comments for current user" });
            }
            return Json(response);
        }

        /// <summary>
        /// Get the comment count for the selected assignment
        /// </summary>
        /// <param name="studentAssignmentId"></param>
        /// <returns></returns>
        public JsonResult GetCommentCountForAssignment(int studentAssignmentId)
        {
            response = new JsonResponse<Comment>();
            comments = commentRepo.GetAll();

            foreach(Comment c in comments)
            {
                if(c.FK_STUDENT_ASSIGNMENT.ID == studentAssignmentId)
                {
                    response.Data.Add(c);
                }
            }
            if(!response.Success)
            {
                response.Error.Add(new Error() { Name = "No Comments", Description = "No comments for that assignment" });
            }
            return Json(response);
        }

        /// <summary>
        /// Create a new comment
        /// </summary>
        /// <param name="studentAssignmentId"></param>
        /// <param name="commentContent"></param>
        /// <returns></returns>
        public async Task<JsonResult> CreateComment(int studentAssignmentId, string commentContent)
        {
            user = await userManager.GetUserAsync(HttpContext.User);
            studentAssignmentRepo.FindByID(studentAssignmentId);
            response = new JsonResponse<Comment>();
            if(user == null)
            {
                response.Error.Add(new Error() { Name = "No User", Description = "No user logged in.  Please login" });
                return Json(response);
            }
            if(studentAssignment == null)
            {
                response.Error.Add(new Error() { Name = "No Assignment", Description = "No student assignment for the given Id" });
                return Json(response);
            }
            if(commentContent == null)
            {
                response.Error.Add(new Error() { Name = "No comment content", Description = "No comment content. Please add a comment body" });
                return Json(response);
            }
            Comment comment = new Comment(user, studentAssignment, System.DateTime.Now, commentContent);
            response.Data.Add(commentRepo.Add(comment));
            if(!response.Success)
            {
                response.Error.Add(new Error() { Name = "Comment not added", Description = "The comment was not added!" });
            }
            return Json(response);

        }

        /// <summary>
        /// Delete a comment from the database
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public JsonResult DeleteComment(int commentId)
        {
            response = new JsonResponse<Comment>();
            comment = commentRepo.FindByID(commentId);
            if(comment == null)
            {
                response.Error.Add(new Error() { Name = "Invalid Id", Description = "No Comment for given Id" });
                return Json(response);
            }
            if (!commentRepo.Delete(comment))
                response.Error.Add(new Error() { Name = "Not Deleted", Description = "Comment not deleted" });
            response.Data.Add(comment);
            return Json(response);
        }
    }
}
