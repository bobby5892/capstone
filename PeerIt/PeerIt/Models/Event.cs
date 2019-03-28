using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeerIt.Models
{
    public class Event
    {
        #region Constructors
        public Event(string AppUserID, string Contents) 
        {

        }
        #endregion Constructors

        #region Variables and Properties
        public int EventID { get; set; }
        public string FK_AppUserID { get; set; }
        private string Content { get; set; }
        private DateTime TimeStampCreated { get; set; }
        private bool HasSeen { get; set; }
        #endregion Variables and Properties

        #region Methods

        // All methods are returning placeholders

        public bool MarkSeen() 
        {
            return false;
        }
        public string GetUser() 
        {
            return "user";
        }
        public string GetContents() 
        {
            return "contents";
        }
        // change return type to a date when ready to implement
        public void GetCreatedTimestamp() 
        {
            
        }
        public bool GetSeen() 
        {
            return false;
        }
        #endregion Methods
    }
}
