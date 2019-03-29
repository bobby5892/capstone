﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace PeerIt.Models
{
    public class CourseGroup
    {
        #region Constructors
        public CourseGroup(int courseID, string AppUserID) 
        {
            // Lookup Course
            // Lookup User

        }
        #endregion Constructors

        #region Variables and Properties
        public int ID { get; set; }

        [Required]        
        public Course FK_Course { get; set; }

        [Required]        
        public AppUser FK_AppUser { get; set; }
        #endregion Variables and Properties
    }
}
