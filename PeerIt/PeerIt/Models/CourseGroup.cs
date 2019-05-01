using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace PeerIt.Models
{
    // https://www.learnentityframeworkcore.com/configuration/many-to-many-relationship-configuration
    /// <summary>
    /// Course Group is a bridging entity that shows what individual user is
    /// in what individual Course. This also contains an attribute for the 
    /// user's review group.
    /// </summary>
    public class CourseGroup
    {
        public int ID { get; set; }
        [Required]
        public Course FK_Course { get; set; }
        [Required]
        public AppUser FK_AppUser { get; set; }

        /// <summary>
        /// This attribute contains the identifier for the User's review group.
        /// The format for the string is "[courseID]-[groupID]", where
        /// [courseID] is the ID for the course contained in FK_Course,
        /// and [groupID] is the group number assigned by the instructor.
        /// </summary>
        public string ReviewGroup { get; set; }
   }
}
