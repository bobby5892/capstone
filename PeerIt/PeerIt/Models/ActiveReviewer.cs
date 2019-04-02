using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using PeerIt.Interfaces;
namespace PeerIt.Models
{
    public class ActiveReviewer : IGenericRepository<ActiveReviewer, int>
    {
        public ActiveReviewer(){}
        public int ID { get; set; }
         [Required]
        public AppUser FK_APP_USER_VIEWER { get; set; }
         [Required]
        public StudentAssignment FK_STUDENT_ASSIGNMENT { get; set; }
        
        public ActiveReviewer(string app_user, int studentAssignment) {
        	// Lookup User - its an ID
        	// Lookup Student Assignment - its an ID

        }

        public ActiveReviewer FindByID(int ID)
        {
            throw new NotImplementedException();
        }

        public List<ActiveReviewer> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool Edit(ActiveReviewer model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(ActiveReviewer model)
        {
            throw new NotImplementedException();
        }

        public ActiveReviewer Add(ActiveReviewer model)
        {
            throw new NotImplementedException();
        }
    }
}
