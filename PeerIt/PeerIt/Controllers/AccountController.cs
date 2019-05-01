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
        private RoleManager<IdentityRole> roleManager;
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;
        private bool isAdmin;
        private bool isInstructor;
        private bool isStudent;
        /// <summary>
        /// Account Controller - Constructor that accepts a userMgr and SigninMgr - Started via startup.cs
        /// </summary>
        /// <param name="userMgr">User Manager</param>
        /// <param name="signinMgr">Sign in Manager</param>
        /// <param name="roleManager">Role Manager</param>
        public AccountController(
                UserManager<AppUser> userMgr,
                SignInManager<AppUser> signinMgr,
                RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userMgr;
            this.signInManager = signinMgr;
            this.roleManager = roleManager;
            //this.isAdmin = HttpContext.User.IsInRole("Administrator");
            //this.isInstructor = HttpContext.User.IsInRole("Instructor");
            //this.isStudent = HttpContext.User.IsInRole("Student");
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
            else
            {
                response.Error.Add(new Error() { Name = "login", Description = "Invalid user name or password" });
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

        /// <summary>
        /// Create a new account.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="emailAddress"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
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
        // Method incomplete - needs to store file
        [HttpPost]
        [Authorize(Roles = "Administrator,Instructor,Student")]
        public async Task<JsonResult> UploadProfilePicture(string userID, string photo) 
        {
            JsonResponse<AppUser> response = new JsonResponse<AppUser>();
            AppUser user = await userManager.FindByIdAsync(userID);
            bool hasAccess = false;
            if (isAdmin)
            {
                hasAccess = true;
            }
            // If its them they can change it
            if (userID == (await userManager.GetUserAsync(HttpContext.User)).Id)
            {
                hasAccess = true;
            }
            // No access
            if (!hasAccess)
            {
                response.Error.Add(new Error() { Name = "UploadProfilePicture", Description = "No access to upload picture" });
                return Json(response);
            }

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
        [Authorize(Roles = "Administrator,Instructor,Student")]
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
        //[Authorize(Roles = "Administrator,Instructor,Student")]
        //[HttpOptions,HttpGet]
        public async Task<JsonResult> GetCurrentUserRole()
        {
            JsonResponse<LoginResponse> response = new JsonResponse<LoginResponse>();
            AppUser currentUser = await userManager.GetUserAsync(HttpContext.User);
            if(currentUser == null)
            {
                response.Error.Add(new Error() { Name = "getRole", Description = "User not logged in" });
            }
            else { 
                var roles = await userManager.GetRolesAsync(currentUser);
                response.Data.Add(new LoginResponse() { EmailAddress=currentUser.Email,Role= roles.FirstOrDefault() });
            }
            return Json(response);
        }

        /// <summary>
        /// Returns the data to populate the form inside the "UpdateAccountWindow"
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> PopulateFormData()
        {
            JsonResponse<AppUser> response = new JsonResponse<AppUser>();
            AppUser user = await userManager.GetUserAsync(HttpContext.User);

            response.Data.Add(user);
            return Json(response);     
        }

        /// <summary>
        /// Edit and update the current users accoun info and return it
        /// </summary>
        /// <param name="email"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<JsonResult> UpdateAccountInfo(string email, string firstName, string lastName, string password)
        {
            JsonResponse<AppUser> response = new JsonResponse<AppUser>();
            AppUser user = await userManager.GetUserAsync(HttpContext.User);
            user.FirstName = firstName;
            user.LastName = lastName;
            if ((password != null) && (password.Length > 1))
            {
                user.PasswordHash = userManager.PasswordHasher.HashPassword(user, password);
            }
            IdentityResult result = await userManager.UpdateAsync(user);
            response.Data.Add(user);
            response.Error.Add(new Error() { Name = "Password Changed", Description = "Password Change Success" });
            return Json(response);


        }
        #endregion Methods that return Json
    }
}
