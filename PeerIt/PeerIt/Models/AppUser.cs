using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;

namespace PeerIt.Models
{
    public class AppUser : IdentityUser
    {
        public DateTime TimestampCreated {get; set;}

        
        [Required(ErrorMessage ="First Name is a required field"),StringLength(25, ErrorMessage = "Must be less than 25 characters")]                    
        public string FirstName { get; set; }
        
        
		[Required(ErrorMessage ="LastName is a required field"),StringLength(25, ErrorMessage = "Must be less than 25 characters")]                             
        public string LastName { get; set; }
        
	   [StringLength(20, ErrorMessage = "Must be less than 20 characters")]                            
       public string PictureFilename { get; set; }

        [Required]
        public bool IsEnabled { get; set; }
    }
}
