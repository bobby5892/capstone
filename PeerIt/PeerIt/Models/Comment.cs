using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace PeerIt.Models
{
    /// <summary>
    /// Comment Data Model
    /// </summary>
    public class Comment
    {
        #region Variables and Properties
        /// <summary>
        /// Auto property Id
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Foreign Key of the user
        /// </summary>
        [Required]
        public AppUser FK_APP_USER { get; set; }
        
        /// <summary>
        /// Foreign key of a student assignment
        /// </summary>
        [Required]        
        public StudentAssignment FK_STUDENT_ASSIGNMENT { get; set; }

        /// <summary>
        /// The time of creation
        /// </summary>
        public DateTime TimestampCreated { get; set; }

        /// <summary>
        /// The content of the comment
        /// </summary>
        [Required(ErrorMessage ="Content is a required field"),StringLength(1000, ErrorMessage = "Must be less than 1000 characters")]                    
        public string Content { get; set; }
        #endregion Variables and Properties

        /// <summary>
        /// Default Constructor
        /// </summary>
        public Comment() { }

        /// <summary>
        /// Costructor that takes all properties
        /// </summary>
        public Comment(AppUser user, StudentAssignment studentAssignment, DateTime timeStamp, string theContent)
        {
            FK_APP_USER = user;
            FK_STUDENT_ASSIGNMENT = studentAssignment;
            TimestampCreated = timeStamp;
            Content = theContent;
        }
    }
}
