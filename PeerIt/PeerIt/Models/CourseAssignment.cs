using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PeerIt.Models
{
    public class CourseAssignment
    {
        #region Constructors
        public CourseAssignment()
        {

        }
        public CourseAssignment(string name, int courseID) 
        {
            this.Name = name;
            // Lookup Course
        }
        #endregion Constructors
        #region Variables and Properties
        public int ID { get; set; }
        
        
        [Required(ErrorMessage ="Name is a required field"),StringLength(60, ErrorMessage = "Must be less than 60 characters")]            
        public string Name { get; set; }


        [Required]        
        public Course FK_COURSE { get; set; } 


        [StringLength(100000, ErrorMessage = "Must be less than 100,000 characters")]            
        public string InstructionText { get; set; }
        
        [StringLength(200, ErrorMessage = "Must be less than 200 characters")]
        public string InstructionsUrl { get; set; }
        
        [StringLength(100000, ErrorMessage = "Must be less than 100,000 characters")]
        public string RubricText { get; set; }

        [StringLength(200, ErrorMessage = "Must be less than 200 characters")]
        public string RubricUrl { get; set; }
        #endregion Variables and Properties
    }
}
