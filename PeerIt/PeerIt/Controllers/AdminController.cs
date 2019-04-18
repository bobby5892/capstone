using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using PeerIt.Models;
using PeerIt.ViewModels;

namespace PeerIt.Controllers
{
    /// <summary>
    /// Admin Controller
    /// </summary>
    public class AdminController : Controller
    {
        private UserManager<AppUser> userManager;
        private IUserValidator<AppUser> userValidator;
        private IPasswordValidator<AppUser> passwordValidator;
        private IPasswordHasher<AppUser> passwordHasher;
        /// <summary>
        /// Admin Controller
        /// </summary>
        /// <param name="usrMgr">UserManager</param>
        /// <param name="userValid">UserValidator</param>
        /// <param name="passValid">PasswordValidator</param>
        /// <param name="passwordHash">PasswordHasher</param>
        public AdminController(UserManager<AppUser> usrMgr,
                IUserValidator<AppUser> userValid,
                IPasswordValidator<AppUser> passValid,
                IPasswordHasher<AppUser> passwordHash)
        {
            userManager = usrMgr;
            userValidator = userValid;
            passwordValidator = passValid;
            passwordHasher = passwordHash;
        }
        [Authorize(Roles = "Administrator")]
       /// Return a list of admins
        public JsonResult Index() {
            JsonResponse<AppUser> response =  new JsonResponse<AppUser>();
            response.Data = userManager.Users.ToList<AppUser>();
            return Json(response);
        }
       

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(CreateModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    UserName = model.Name,
                    Email = model.Email
                };
                IdentityResult result
                    = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(string id)
        {
            JsonResponse<AppUser> response = new JsonResponse<AppUser>();
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                  
                    return Json(response);
                }
                else
                {
                    response.Error.Add(new Error() { Name = "Admin", Description = "Unable to delete user" });
                    return Json(response);
                }
            }
           
            response.Error.Add(new Error() { Name = "Admin", Description = "User Not Found" });
            return Json(response);
        }
      

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<JsonResult> Edit(string id, string email,
                string password)
        {
            JsonResponse<AppUser> response = new JsonResponse<AppUser>();

            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.Email = email;
                IdentityResult validEmail
                    = await userValidator.ValidateAsync(userManager, user);
                if (!validEmail.Succeeded)
                {
                    response.Error.Add(new Error() { Name = "Admin", Description ="Invalid Email Address" });
                    return Json(response);
                }
                IdentityResult validPass = null;
                if (!string.IsNullOrEmpty(password))
                {
                    validPass = await passwordValidator.ValidateAsync(userManager,
                        user, password);
                    if (validPass.Succeeded)
                    {
                        user.PasswordHash = passwordHasher.HashPassword(user,
                            password);
                    }
                    else
                    {
                        response.Error.Add(new Error() { Name = "Admin", Description = "Invalid Password" });
                        return Json(response);
                    }
                
                }
                if ((validEmail.Succeeded && validPass == null)
                        || (validEmail.Succeeded
                        && password != string.Empty && validPass.Succeeded))
                {
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        response.Data.Add(user);
                        return Json(response);
                    }
                }
                else
                {
                    response.Error.Add(new Error() { Name = "Admin", Description = "Unable to Edit User" });
                    return Json(response);
                }
                
            }
            response.Error.Add(new Error() { Name = "Admin", Description = "Unable to find user" });
            return Json(response);
        }

      

        #region Methods that return Json

        [HttpGet]
        public JsonResult GetSettings() 
        {
            return Json(false);
        }

        public JsonResult CreateInstructor() 
        {
            return null;
        }

        #endregion Methods that return Json
    }
}
