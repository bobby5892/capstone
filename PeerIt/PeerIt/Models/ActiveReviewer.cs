using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PeerIt.Interfaces;
namespace PeerIt.Models
{
    public class ActiveReviewer 
    {

        public int ID { get; set; }
         [Required]
        public AppUser FK_APP_USER_VIEWER { get; set; }
         [Required]
        public StudentAssignment FK_STUDENT_ASSIGNMENT { get; set; }
      
    }
}
