using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PeerIt.Models;
using PeerIt.Repositories;
using PeerIt.ViewModels;

namespace PeerIt.Controllers
{
    public class CommentController : Controller
    {
        private CommentRepository commentRepo;
        private JsonResponse<Comment> response;

        public CommentController()
        {

        }
        public JsonResult GetCommentsByUserId(string userId)
        {
            return Json(response);
        }
        public JsonResult GetCommentsByAssignmentId(int studentAssignmentId)
        {
            return Json(response);
        }
        public JsonResult GetCommentCountForUser(string userId)
        {
            return Json(response);
        }
        public JsonResult GetCommentCountForAssignment(int studentAssignmentId)
        {
            return Json(response);
        }
        public JsonResult CreateComment(string userID, int assignment_id, string comment)
        {
            return Json(response);
        }
        public JsonResult DeleteComment(int commentId)
        {
            return Json(response);
        }
    }

}
