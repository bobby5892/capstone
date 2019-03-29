using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PeerIt.Models
{
    public class ForgotPassword
    {
        public int ID { get; set; }

        [Required]        
        public AppUser FK_APP_USER { get; set; }

        [Required]        
        public string ResetHash { get; set; }


        private string generateHash()
        {
            return "super secret password hash";
        }
    }
}
