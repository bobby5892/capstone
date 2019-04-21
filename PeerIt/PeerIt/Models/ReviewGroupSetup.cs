using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeerIt.Models
{
    /// <summary>
    /// This model class associates a Review Group to a Course Assignment.
    /// </summary>
    public class ReviewGroupSetup
    {
        /// <summary>
        /// The primary key for the ReviewGroupSetup
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// This is the associated Course Assignment for the ReviewGroups
        /// to be associated to.
        /// </summary>
        public CourseAssignment CourseAssignment { get; set; }
    }
}
