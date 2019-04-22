using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PeerIt.Models
{
    public class Event
    {
        #region Variables and Properties
        public int ID { get; set; }

        [Required]        
        public AppUser FK_AppUser { get; set; }

        [Required(ErrorMessage ="Content is a required field"),StringLength(5000, ErrorMessage = "Must be less than 5,000 characters")]            
        public string Content { get; set; }
        
        public DateTime TimeStampCreated { get; set; }

        [Required]        
        public bool HasSeen { get; set; }
        #endregion Variables and Properties
    }
}
