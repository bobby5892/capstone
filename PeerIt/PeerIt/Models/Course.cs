using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace PeerIt.Models
{
    public class Course
    {
        #region Constructors
        public Course()
        {

        }
        public Course(string name, bool isActive, int InstructorID) 
        {
            this.Name = name;
            this.IsActive = isActive;
            // Lookup instructor
           
        }
        #endregion Constructors

        #region Variables and Properties
        public int ID { get; set; }

        [Required(ErrorMessage ="Name is a required field"),StringLength(60, ErrorMessage = "Must be less than 60 characters")]                    
        public string Name { get; set; }

        [Required]        
        public bool IsActive { get; set; }

        [Required]            
        public AppUser FK_INSTRUCTOR {get; set;}

        #endregion Variables and Properties

    }
}
