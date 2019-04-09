using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeerIt.Infrastructure
{
    public static class Infastrucure
    {
 
    /// <summary>
    /// Roles enum
    /// </summary>
        public enum Roles
        {
            /// <summary>
            /// Admin
            /// </summary>
            ADMIN,
            /// <summary>
            /// Student
            /// </summary>
            STUDENT,
            /// <summary>
            /// Instructor
            /// </summary>
            INSTRUCTOR,
            /// <summary>
            /// InvitedUser
            /// </summary>
            INVITEDUSER
        }
    }
}
