using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeerIt.Models
{
    public class CourseGroup
    {
        #region Constructors
        public CourseGroup(int courseID, string AppUserID) 
        {

        }
        #endregion Constructors

        #region Variables and Properties
        public int CourseGroupID { get; set; }
        public int FK_CourseID { get; set; }
        public string FK_AppUserID { get; set; }
        #endregion Variables and Properties
    }
}
