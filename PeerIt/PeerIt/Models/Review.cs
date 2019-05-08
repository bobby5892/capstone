using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PeerIt.Models
{
    /// <summary>
    /// Review
    /// </summary>
    public class Review
    {
        /// <summary>
        /// Id 
        /// </summary>
    	public int ID {get; set;}

        /// <summary>
        /// FK_PFile
        /// </summary>
        [Required]
        public PFile FK_PFile { get; set; }

    	[Required]
    	public AppUser FK_APP_USER {get; set;}

    	[Required]    	
    	public StudentAssignment FK_STUDENT_ASSIGNMENT {get; set;}

    	public DateTime TimestampCreated {get; set;}
    }
}
