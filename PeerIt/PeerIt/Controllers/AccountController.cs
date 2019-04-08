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
    /// A Controller that handles all generic account related activities.
    /// </summary>
    public class AccountController : Controller
    {
        #region Private Repository Variables

        //private IGenericRepository<Event, int> eventRepository;
        //private IGenericRepository<ForgotPassword, int> forgotPasswordRepository;

        #endregion Private Repository Variables

        #region Constructors

        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;
        /// <summary>
        /// Account Controller - Constructor that accepts a userMgr and SigninMgr - Started via startup.cs
        /// </summary>
        /// <param name="userMgr"></param>
        /// <param name="signinMgr"></param>
        public AccountController(UserManager<AppUser> userMgr,
                SignInManager<AppUser> signinMgr)
        {
            userManager = userMgr;
            signInManager = signinMgr;
        }

        /// <summary>
        /// Handle Login Redirection
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        // Needs work!
        public JsonResult Login(string returnUrl)
        {
            JsonResponse<bool> response = new JsonResponse<bool>();
            ViewBag.returnUrl = returnUrl;
            return Json(response);
        }
        /// <summary>
        /// Do Login
        /// </summary>
        /// <param name="details"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Login(LoginModel details,
                string returnUrl)
        {
            JsonResponse<AppUser> response = new JsonResponse<AppUser>();

            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByEmailAsync(details.Email);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result =
                            await signInManager.PasswordSignInAsync(
                                user, details.Password, false, false);
                    
                    if (result.Succeeded)
                    {
                        response.Data.Add(user);
                        return Json(response);
                    }
                }
                ModelState.AddModelError(nameof(LoginModel.Email),
                    "Invalid user or password");
            }

            response.Error.Add(new Error("FailedLogin", "Invalid Username or Password"));

            return Json(response);
        }
        /// <summary>
        /// Handle Logout
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<JsonResult> Logout()
        {
            JsonResponse<AppUser> response = new JsonResponse<AppUser>();
            await signInManager.SignOutAsync();
            return Json(response);
        }
    /// <summary>
    ///  Show access Denied
    /// </summary>
    /// <returns></returns>
        [AllowAnonymous]
        public JsonResult AccessDenied()
        {
            JsonResponse<bool> response = new JsonResponse<bool>();
            response.Error.Add(new Error("AccessDenied", "You are not allowed here Naive."));
            return Json(response);
        }

        #endregion Constructors

        #region Methods that return Json
        /// <summary>
        /// Create a new account
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="emailAddress"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CreateAccount(string firstName, string lastName, 
                                        string emailAddress, string password)
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


        #endregion Methods that return Json
    }
}
