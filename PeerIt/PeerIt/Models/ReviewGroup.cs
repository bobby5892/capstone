using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeerIt.Models
{
    /// <summary>
    /// This model class represents an instance of a single person in a single
    /// review group.
    /// </summary>
    public class ReviewGroup
    {
        /// <summary>
        /// The primary key for a ReviewGroup Record.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The User being assiciated with a group of reviewers.
        /// </summary>
        public AppUser AppUser { get; set; }

        /// <summary>
        /// The ID for the review group the associated user is involved with.
        /// </summary>
        public int groupID { get; set; }
    }
}
