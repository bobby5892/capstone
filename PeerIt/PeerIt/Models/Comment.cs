using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeerIt.Models
{
    public class Comment
    {
        #region Constructors

        #endregion Constructors

        #region Variables and Properties
        public int CommentID { get; set; }
        private string FK_AppUserID { get; set; }
        private int FK_StudentAssignmentID { get; set; }
        private DateTime TimeStampCreated { get; set; }
        private string Contents { get; set; }
        #endregion Variables and Properties

        #region Methods

        // All methods currently return placeholders

        public string GetStudent() 
        {
            return "name";
        }
        public int GetStudentAssignmentID() 
        {
            return -1;
        }
        // change return type to a date when ready to implement
        public void GetTimeStamp() 
        {
            
        }
        public string GetContents() 
        {
            return "contents";
        }
        #endregion Methods
    }
}
