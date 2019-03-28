using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeerIt.Models
{
    public class ActiveReviewer
    {
        public int ID { get; set; }
        public AppUser FK_APP_USER_VIEWER { get; set; }
        public StudentAssignment FK_STUDENT_ASSIGNMENT_ID { get; set; }
        public ActiveReviewer(string App_user, int StudentAssignment) {

        }
    }
}
