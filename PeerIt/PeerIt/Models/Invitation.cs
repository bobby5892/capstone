using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PeerIt.Models
{
    public class Invitation
    {
        public int ID { get; set; }

        [Required]        
        public Course FK_COURSE { get; set; }

        [Required]
        public AppUser FK_APP_USER_SENDER { get; set; }

        [Required]        
        public AppUser FK_APP_USER_RECIPIENT { get; set; }

        [Required]
        public string InvitationHash { get; set; }

        public bool CheckHash(string password)
        {
            return false;
        }
    }
}
