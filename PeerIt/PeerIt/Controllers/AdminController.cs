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
                
                IPasswordHasher<AppUser> passwordHash)
        {
            userManager = usrMgr;
            userValidator = userValid;
            //passwordValidator = passValid;
            passwordHasher = passwordHash;
        }
        /// <summary>
        /// Get All Administrators
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<JsonResult> GetAdministrators() {
            JsonResponse<AppUser> response =  new JsonResponse<AppUser>();
            var allUsers = userManager.Users.ToList<AppUser>();
            // Asp does not support an async lambda to a FIND for C# so using the usermanager to check the is in role Async all in 1 fell swoop - is not allowed
            //https://stackoverflow.com/questions/17226284/convert-async-lambda-expression-to-delegate-type-system-funct
           
            for(int i=0;i < allUsers.Count; i++)
            {
                if (await userManager.IsInRoleAsync(allUsers[i], "Administrator")){
                    response.Data.Add(allUsers[i]);
                }
            }

            return Json(response);
        }

        /// <summary>
        /// Get All Instructor
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<JsonResult> GetInstructors()
        {
            JsonResponse<AppUser> response = new JsonResponse<AppUser>();
            var allUsers = userManager.Users.ToList<AppUser>();
            // Asp does not support an async lambda to a FIND for C# so using the usermanager to check the is in role Async all in 1 fell swoop - is not allowed
            //https://stackoverflow.com/questions/17226284/convert-async-lambda-expression-to-delegate-type-system-funct

            for (int i = 0; i < allUsers.Count; i++)
            {
                if (await userManager.IsInRoleAsync(allUsers[i], "Instructor"))
                {
                    response.Data.Add(allUsers[i]);
                }
            }

            return Json(response);
        }
        /// <summary>
        /// Get All Student
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<JsonResult> GetStudents()
        {
            JsonResponse<AppUser> response = new JsonResponse<AppUser>();
            var allUsers = userManager.Users.ToList<AppUser>();
            // Asp does not support an async lambda to a FIND for C# so using the usermanager to check the is in role Async all in 1 fell swoop - is not allowed
            //https://stackoverflow.com/questions/17226284/convert-async-lambda-expression-to-delegate-type-system-funct

            for (int i = 0; i < allUsers.Count; i++)
            {
                if (await userManager.IsInRoleAsync(allUsers[i], "Student"))
                {
                    response.Data.Add(allUsers[i]);
                }
            }

            return Json(response);
        }
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<JsonResult> GetInvitedGuests()
        {
            JsonResponse<AppUser> response = new JsonResponse<AppUser>();
            var allUsers = userManager.Users.ToList<AppUser>();
            // Asp does not support an async lambda to a FIND for C# so using the usermanager to check the is in role Async all in 1 fell swoop - is not allowed
            //https://stackoverflow.com/questions/17226284/convert-async-lambda-expression-to-delegate-type-system-funct

            for (int i = 0; i < allUsers.Count; i++)
            {
                if (await userManager.IsInRoleAsync(allUsers[i], "InvitedGuest"))
                {
                    response.Data.Add(allUsers[i]);
                }
            }

            return Json(response);
        }
        /// <summary>
        /// This is the create a user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<JsonResult> Create(CreateModel model)
        //public async Task<JsonResult> Create(CreateModel model)
        {
            JsonResponse<AppUser> response = new JsonResponse<AppUser>();
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser {
                    FirstName = model.FirstName.ToLower(),
                    LastName = model.LastName.ToLower(),
                    UserName = model.Email.ToLower(),
                    Email = model.Email.ToLower(),
                    IsEnabled = true,
                    EmailConfirmed = true,
                    LockoutEnabled = false
                    

                };

                IdentityResult result
                    = await userManager.CreateAsync(user, model.Password);

                
                if (result.Succeeded)
                {
                    AppUser newUser = await userManager.FindByEmailAsync(user.Email);
                    response.Data.Add(newUser);
                    // Now lets add them as an invited user
                    await this.userManager.AddToRoleAsync(newUser, "InvitedGuest");
                    return Json(response);
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        response.Error.Add(new Error() { Name = "admin", Description = error.Description.ToString() });
                        
                    }
                }
            }
            else
            {
                response.Error.Add(new Error() { Description = "Failed Validation", Name = "AdminCreate" });
            }
            return Json(response);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(string userID)
        {
            JsonResponse<AppUser> response = new JsonResponse<AppUser>();
            AppUser user = await userManager.FindByIdAsync(userID);
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
      
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<JsonResult> GetUser(string id)
        {
            JsonResponse<EditModel> response = new JsonResponse<EditModel>();
            AppUser user = await userManager.FindByIdAsync(id);
            if(user == null)
            {
                response.Error.Add(new Error() { Description = "No such user", Name = "adminGetUser" });
                return Json(response);
            }
            var roles = await userManager.GetRolesAsync(user);
            response.Data.Add(new EditModel() {
                    Email=user.Email,
                    IsEnabled =user.IsEnabled,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ID = user.Id,
                    Role = roles[0]
            });
            return Json(response);
        }
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<JsonResult> Edit(EditModel model)
        {
            
            JsonResponse<AppUser> response = new JsonResponse<AppUser>();

            AppUser user = await userManager.FindByIdAsync(model.ID);
            if (user != null)
            {
                user.Email = model.Email;
                IdentityResult validEmail
                    = await userValidator.ValidateAsync(userManager, user);
                if (!validEmail.Succeeded)
                {
                    response.Error.Add(new Error() { Name = "Admin", Description ="Invalid Email Address" });
                    return Json(response);
                }

                if (ModelState.IsValid)
                {
                    if(model.Email.Length > 1) { user.Email = model.Email; user.NormalizedEmail = model.Email; user.UserName = model.Email; }
                    if((model.Password != null) && (model.Password.Length > 1)) { user.PasswordHash = userManager.PasswordHasher.HashPassword(user, model.Password); }
                    if(model.FirstName.Length > 1) { user.FirstName = model.FirstName; }
                    if(model.LastName.Length > 1) { user.LastName = model.LastName; }
                    if(model.Role.Length > 1)
                    {
                        var roles = await userManager.GetRolesAsync(user);
                        await userManager.RemoveFromRolesAsync(user, roles.ToArray());

                        // Now lets add the role we want
                        await this.userManager.AddToRoleAsync(user, model.Role);
                    }
                    user.IsEnabled = model.IsEnabled;

                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        response.Data.Add(user);
                        return Json(response);
                    }
                    else
                    {
                        response.Error.Add(new Error() { Name = "Admin", Description = "Unable to Edit User" });
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
        [HttpPatch]
        public async Task<bool>  SetRole(string userID, string role)
        {
            List<string> Roles = new List<string>() { "Instructor", "Student", "Administrator", "InvitedGuest" };
            // check if the role suggested exists
            if (!Roles.Exists(
                  c =>  { if (c == role){ return true;} return false; } )){
                // Failed to change
                return false;
            }

            AppUser lookupUser = await userManager.FindByIdAsync(userID);
            // Check if user exists
            if (lookupUser == null)
            {
                return false;
            }

            // strip all roles from a user -- https://stackoverflow.com/questions/41074595/remove-all-roles-from-a-user-mvc-5
            var roles = await userManager.GetRolesAsync(lookupUser);
            await userManager.RemoveFromRolesAsync(lookupUser, roles.ToArray());

            // Now lets add the role we want
             await this.userManager.AddToRoleAsync(lookupUser, role);
            // Role request done
             return true;


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
