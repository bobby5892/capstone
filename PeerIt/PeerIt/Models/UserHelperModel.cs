using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PeerIt.Models
{
    /// <summary>
    /// CreateModel - used when creating new user
    /// </summary>
    public class CreateModel
    {
        //https://github.com/Apress/pro-asp.net-core-mvc-2/blob/master/29%20-%20Applying%20Identity/Users/Users/Models/UserViewModels.cs
        /// <summary>
        /// Name of User
        /// </summary>
        [Required]

        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// Email address of user
        /// </summary>
        [Required]
        public string Email { get; set; }
        /// <summary>
        /// Password of user
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
    /// <summary>
    /// LoginModel - used when validating forms
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// Email Address
        /// </summary>
        [Required]
        [UIHint("email")]
        public string Email { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        [Required]
        [UIHint("password")]
        public string Password { get; set; }
    }
    /// <summary>
    /// RoleEditModel - used to hold role data
    /// </summary>
    public class RoleEditModel
    {
        /// <summary>
        /// Role
        /// </summary>
        public IdentityRole Role { get; set; }
        /// <summary>
        /// Members - AppUsers
        /// </summary>
        public IEnumerable<AppUser> Members { get; set; }
        /// <summary>
        /// Non Members - AppUsers
        /// </summary>
        public IEnumerable<AppUser> NonMembers { get; set; }
    }
    /// <summary>
    /// roleModificationModel - used to hold changes to Roles
    /// </summary>
    public class RoleModificationModel
    {
        /// <summary>
        /// Name of Role
        /// </summary>
        [Required]
        public string RoleName { get; set; }
        /// <summary>
        /// CLSID of Role
        /// </summary>
        public string RoleId { get; set; }
        /// <summary>
        /// List to Add
        /// </summary>
        public string[] IdsToAdd { get; set; }
        /// <summary>
        /// List to Delete
        /// </summary>
        public string[] IdsToDelete { get; set; }
    }
    public class EditModel
    {
        //https://github.com/Apress/pro-asp.net-core-mvc-2/blob/master/29%20-%20Applying%20Identity/Users/Users/Models/UserViewModels.cs
        /// <summary>
        /// Name of User
        /// </summary>
        /// 
        [Required]
        public string ID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        /// <summary>
        /// Email address of user
        /// </summary>
        [Required]
        public string Email { get; set; }
        /// <summary>
        /// Password of user
        /// </summary>
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }

        [Required]
        public bool IsEnabled { get; set; }
    }
}
