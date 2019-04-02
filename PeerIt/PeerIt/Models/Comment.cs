using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace PeerIt.Models
{
    public class Comment
    {
        #region Constructors

        #endregion Constructors

        #region Variables and Properties
        public int ID { get; set; }

        [Required]
        public AppUser FK_APP_USER { get; set; }
        
        [Required]        
        public StudentAssignment FK_STUDENT_ASSIGNMENT { get; set; }

        public DateTime TimestampCreated { get; set; }

        [Required(ErrorMessage ="Content is a required field"),StringLength(1000, ErrorMessage = "Must be less than 1000 characters")]                    
        public string Content { get; set; }
        #endregion Variables and Properties
        public Comment()
        {

        }
    }
}
