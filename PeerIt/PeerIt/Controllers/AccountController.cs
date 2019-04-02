using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PeerIt.Models;
using PeerIt.Repositories;
using PeerIt.Interfaces;

namespace PeerIt.Controllers
{
    public class AccountController : Controller
    {
        #region Private Repository Variables
        
        //private IGenericRepository<Event, int> eventRepository;
        //private IGenericRepository<ForgotPassword, int> forgotPasswordRepository;

        #endregion Private Repository Variables

        #region Constructors

        public AccountController() 
        {

        }

        #endregion Constructors

        #region Methods that return Json

        [HttpPost]
        public JsonResult CreateAccount(string firstName, string lastName, 
                                        string emailAddress, string password)
        {
            return Json(false);
        }

        [HttpPost]
        public JsonResult Login(string emailAddress, string password) 
        {
            return Json(false);
        }

        [HttpPost]
        public JsonResult ForgotPassword(string emailAddress) 
        {
            return Json(false);
        }

        [HttpPost]
        public JsonResult ForgotChangePassword(string emailAddress, 
                                               string hashKey,
                                               string newPassword) 
        {
            return Json(false);
        }

        [HttpGet]
        public JsonResult GetAccount(string userID) 
        {
            return Json(false);
        }

        [HttpPut]
        public JsonResult SetFirstName(string userID, string firstName) 
        {
            return Json(false);
        }

        [HttpPut]
        public JsonResult SetLastName(string userID, string lastName) 
        {
            return Json(false);
        }

        [HttpPut]
        public JsonResult SetEmailAddress(string userID, string EmailAddress) 
        {
            return Json(false);
        }

        [HttpPost]
        public JsonResult UploadProfilePicture(string userID, string photo) 
        {
            return Json(false);
        }

        [HttpGet]
        public JsonResult GetRoles() 
        {
            return Json(false);
        }

        [HttpGet]
        public JsonResult Logout() 
        {
            return Json(false);
        }

        #endregion Methods that return Json
    }
}
