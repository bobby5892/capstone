using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace PeerIt.Models
{
    public class CourseDataOut
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public string FK_INSTRUCTOR_NAME { get; set; }
    }
}
