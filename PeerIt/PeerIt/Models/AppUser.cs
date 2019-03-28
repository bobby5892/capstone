using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace PeerIt.Models
{
    public class AppUser : IdentityUser
    {
        private DateTime TimestampCreated {get; set;}
        private string FirstName { get; set; }
        private string LastName { get; set; }
        //Email is already in Identity
        private string PictureFilename { get; set; }
        private bool IsEnabled { get; set; }
        public int GetRole()
        {
            return -1;
        }
        public bool SetRole(int role)
        {
            return false;
        }
        public string GetFirstName()
        {
            return "string";
        }
        public string GetLastName()
        {
            return "string";
        }
        public string GetFullName()
        {
            return "string";
        }
        public string GetEmailAddress()
        {
            return "string";            
        }
        public bool SetFirstName(string FirstName)
        {
            return false;
        }
        public bool ToggleEnabled()
        {
            return false;
        }
        public bool GetEnabled() {
            return false;
        }

    }
}
