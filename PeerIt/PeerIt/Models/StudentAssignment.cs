using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace PeerIt.Models
{
    public class StudentAssignment
    {
        #region Variables and Properties

        public int ID { get; set; }
        
        [Required]
        public CourseAssignment CourseAssignment { get; set; }
        
        [Required]
        public AppUser AppUser { get; set; }

        public DateTime TimestampCreated { get; set; }
        
        [StringLength(100000, ErrorMessage = "Must be less than 100,000 characters")] 
        public string Content { get; set; }

        //[Required(ErrorMessage ="Status is a required field"),StringLength(100, ErrorMessage = "Must be less than 100 characters")]  
        public string Status { get; set; }

        [Required]
        public PFile FK_PFile { get; set; }

        #endregion Variables and Properties
    }
}
