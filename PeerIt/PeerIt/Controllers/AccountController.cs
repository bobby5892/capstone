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
        private RoleManager<IdentityRole> roleManager;
        /// <summary>
        /// Account Controller - Constructor that accepts a userMgr and SigninMgr - Started via startup.cs
        /// </summary>
        /// <param name="userMgr"></param>
        /// <param name="signinMgr"></param>
        public AccountController(UserManager<AppUser> userMgr,
                SignInManager<AppUser> signinMgr, RoleManager<IdentityRole> roleMgr)
        {
            userManager = userMgr;
            signInManager = signinMgr;
            roleManager = roleMgr;
        }

        #endregion Constructors

        #region Methods that return Json

        /// <summary>
        /// Handle Login Redirection
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        // We may not need this method - Javascript potentially can pull up webix form for login.
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

        /// <summary>
        /// Create a new account.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="emailAddress"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> CreateAccount(string firstName, string lastName, 
                                        string emailAddress, string password)
        {
            JsonResponse<AppUser> response = new JsonResponse<AppUser>();

            if (userManager.FindByEmailAsync(emailAddress) != null)
            {
                AppUser newUser = new AppUser
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = emailAddress
                };

                IdentityResult result = await userManager.CreateAsync(newUser, password);

                if (result.Succeeded)
                {
                    response.Data.Add(newUser);
                    return Json(response);
                }
            }
            response.Error.Add(new Error("FailedAccountCreate", "No Account for you"));
            return Json(response);
        }

        /// <summary>
        /// Sends an email to an AppUser's Email address to handle
        /// a forgotten password.
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ForgotPassword(string emailAddress) 
        {
            return Json(false);
        }

        /// <summary>
        /// When an email link is clicked, give the AppUser an interface
        /// to change their password.
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="hashKey"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ForgotChangePassword(string emailAddress, 
                                               string hashKey,
                                               string newPassword) 
        {
            return Json(false);
        }

        /// <summary>
        /// Gets the Account information by the account user's ID.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetAccount(string userID) 
        {
            JsonResponse<AppUser> response = new JsonResponse<AppUser>();
            AppUser appUser = await userManager.FindByIdAsync(userID);

            if (appUser != null)
            {
                response.Data.Add(appUser);
                return Json(response);
            }
            response.Error.Add(new Error("NotFound", "User was not Found"));
            return Json(response);
        }

        /// <summary>
        /// Sets the FirstName property of an AppUser.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="firstName"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<JsonResult> SetFirstName(string userID, string firstName) 
        {
            JsonResponse<AppUser> response = new JsonResponse<AppUser>();
            AppUser user = await userManager.FindByIdAsync(userID);

            if (user != null)
            {
                user.FirstName = firstName;
                IdentityResult result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    response.Data.Add(user);
                    return Json(response);
                }
            }

            response.Error.Add(new Error("NotFound", "The User was not Found"));
            return Json(response);
        }

        /// <summary>
        /// Sets the LastName property of an AppUser.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<JsonResult> SetLastName(string userID, string lastName) 
        {
            JsonResponse<AppUser> response = new JsonResponse<AppUser>();
            AppUser user = await userManager.FindByIdAsync(userID);

            if (user != null)
            {
                user.LastName = lastName;
                IdentityResult result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    response.Data.Add(user);
                    return Json(response);
                }
            }

            response.Error.Add(new Error("NotFound", "The User was not Found"));
            return Json(response);
        }

        /// <summary>
        /// Sets the Email property of an AppUser.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<JsonResult> SetEmailAddress(string userID, string emailAddress) 
        {
            JsonResponse<AppUser> response = new JsonResponse<AppUser>();
            AppUser user = await userManager.FindByIdAsync(userID);

            if (user != null)
            {
                user.Email = emailAddress;
                IdentityResult result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    response.Data.Add(user);
                    return Json(response);
                }
            }

            response.Error.Add(new Error("NotFound", "The User was not Found"));
            return Json(response);
        }

        /// <summary>
        /// Sets the PictureFilename property of an AppUser.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="photo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> UploadProfilePicture(string userID, string photo) 
        {
            JsonResponse<AppUser> response = new JsonResponse<AppUser>();
            AppUser user = await userManager.FindByIdAsync(userID);

            if (user != null)
            {
                user.PictureFilename = photo;
                IdentityResult result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    response.Data.Add(user);
                    return Json(response);
                }
            }

            response.Error.Add(new Error("NotFound", "The User was not Found"));
            return Json(response);
        }

        /// <summary>
        /// Returns the available roles
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetRoles() 
        {
            JsonResponse<List<IdentityRole>> response = new JsonResponse<List<IdentityRole>>();
            response.Data.Add(roleManager.Roles.ToList());
            return Json(response);
        }

        #endregion Methods that return Json
    }
}
