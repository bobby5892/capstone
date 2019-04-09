﻿using System;
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
        private JsonResponse<Review> response;
        private List<Review> reviews;
        private Review review;
        private UserManager<AppUser> userManager;

        public ReviewController(UserManager<AppUser> userMgr,ReviewRepository repo)
        {
            userManager = userMgr;
            reviewRepo = repo;
        }

        public JsonResult GetReviewsByAssignmentId(int id)
        {
            response = new JsonResponse<Review>();
            reviews = reviewRepo.GetAll();

            foreach (Review r in reviews)
            {
                if (id == r.FK_STUDENT_ASSIGNMENT.ID)
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
        public JsonResult GetReviewById(int id)
        {
            response = new JsonResponse<Review>();
            reviews = reviewRepo.GetAll();

            foreach(Review r in reviews)
            {
                if (r.ID == id)
                    response.Data.Add(r);
            }
            if(response.Success)
            {
                return Json(response);
            }
            response.Error.Add(new Error() { Name = "No Review", Description = "No Review for that Id" });
            return Json(response);
        }
        public string CreateReview(string contents, string userId, int assignmentId)
        {
            //review = new Review() { Content = contents, FK_APP_USER =, FK_STUDENT_ASSIGNMENT = assignmentId}

            return "confirmation or an endpoint?";
        }


    }
}
