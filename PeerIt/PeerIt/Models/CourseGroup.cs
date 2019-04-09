using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace PeerIt.Models
{
    /* Course Group is a bridging entity that shows what individual user is
     * in what individual Course. not to be confused with a Review Group.
     */
    // https://www.learnentityframeworkcore.com/configuration/many-to-many-relationship-configuration
   public class CourseGroup
    {
        public int ID { get; set; }
        [Required]
        public Course FK_Course { get; set; }
        [Required]
        public AppUser FK_AppUser { get; set; }
   }
}
