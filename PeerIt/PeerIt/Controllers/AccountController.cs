using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PeerIt.Models;
using PeerIt.Repositories;
using PeerIt.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using PeerIt.ViewModels;

namespace PeerIt.Controllers
{
    public class AccountController : Controller
    {
        #region Private Repository Variables

        //private IGenericRepository<Event, int> eventRepository;
        //private IGenericRepository<ForgotPassword, int> forgotPasswordRepository;

        #endregion Private Repository Variables

        #region Constructors
        private RoleManager<IdentityRole> roleManager;
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;
        /// <summary>
        /// Account Controller - Constructor that accepts a userMgr and SigninMgr - Started via startup.cs
        /// </summary>
        /// <param name="userMgr"></param>
        /// <param name="signinMgr"></param>
        public AccountController(UserManager<AppUser> userMgr,
                SignInManager<AppUser> signinMgr,
                RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userMgr;
            this.signInManager = signinMgr;
            this.roleManager = roleManager;
        }

        /// <summary>
        /// Handle Login Redirection
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
       /*( [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }*/
        /// <summary>
        /// Do Login
        /// </summary>
        /// <param name="details"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> Login(LoginModel details,
                string returnUrl)
        {
            JsonResponse <LoginResponse> response = new JsonResponse<LoginResponse>();
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
                        var RolesForUser = await userManager.GetRolesAsync(user);
                        response.Data.Add(new LoginResponse()
                        {
                            EmailAddress = user.Email,
                            Role = RolesForUser[0] // Only supporting 1 role per user - so its the first role
                        
                        });
                    }
                    else
                    {
                        //ModelState.AddModelError(nameof(LoginModel.Email),"Invalid user or password");
                        response.Error.Add(new Error() { Name="login",Description="Invalid user name or password"});
                    }
                }
                else
                {
                    response.Error.Add(new Error() { Name = "login", Description = "Invalid user name or password" });
                }
            
            }
            return Json(response);
        }
        /// <summary>
        /// Handle Logout
        /// </summary>
        /// <returns></returns>
        // [Authorize]
        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> Logout()
        {
             await this.signInManager.SignOutAsync();
            
            JsonResponse<bool> response = new JsonResponse<bool>();
            // Since no errors - the status will be true
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
            response.Error.Add(new Error() {
                Name="NoAccess",
                Description="Access Denied"
            });
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
            JsonResponse<IdentityRole> response = new JsonResponse<IdentityRole>();
            var role = this.roleManager.Roles.ToList<IdentityRole>();
            response.Data = role;
            return Json(response);
        }
        /// <summary>
        /// Get the role of the current user
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetCurrentUserRole()
        {
            JsonResponse<string> response = new JsonResponse<string>();
            AppUser currentUser = await userManager.GetUserAsync(HttpContext.User);
            if(currentUser == null)
            {
                response.Error.Add(new Error() { Name = "getRole", Description = "User not logged in" });
            }
            else { 
                var roles = await userManager.GetRolesAsync(currentUser);
                response.Data.Add(roles.FirstOrDefault());
            }
            return Json(response);
        }
        /// <summary>
        /// Temp Dev Command
        /// </summary>
        /// <returns></returns>
        /*[HttpGet]
        public async Task<JsonResult> DevCreateUser()
        {
            AppUser admin = new AppUser()
            {
                Id = "8820ee66-681f-45b3-92ed-7d8dee5d4beb",
                FirstName = "Admin",
                LastName = "Admin",
                SecurityStamp = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("eargFRergijdfoaij34g34")),
                Email = "admin@example.com",
                NormalizedEmail = "admin@example.com",
                LockoutEnabled = false,
                IsEnabled = true,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
                TimestampCreated = DateTime.Now
            };
           admin.PasswordHash = userManager.PasswordHasher.HashPassword(admin, "password");
            var result = await userManager.UpdateAsync(admin);
            if (!result.Succeeded)
            {
                //throw exception......
            }
            return Json("done");
        }
        */


        #endregion Methods that return Json
    }
}
