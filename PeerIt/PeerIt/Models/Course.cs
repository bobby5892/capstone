using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace PeerIt.Models
{
    public class Course
    {
        #region Variables and Properties
        public int ID { get; set; }

        [Required(ErrorMessage ="Name is a required field"),StringLength(60, ErrorMessage = "Must be less than 60 characters")]                    
        public string Name { get; set; }

        [Required]        
        public bool IsActive { get; set; }

        [Required]            
        public AppUser FK_INSTRUCTOR {get; set;}

        /// <summary>
        /// A list of the users in the class
        /// </summary>
       // public ICollection<AppUser> Students { get; set; }
        #endregion Variables and Properties
    }
}
