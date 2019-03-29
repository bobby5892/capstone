using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeerIt.Models;
using PeerIt.Interfaces;
namespace PeerIt.Repositories
{
    public class ActiveReviewerRepository : IGenericRepository<ActiveReviewer, int>
    {
        AppDBContext context;

        public List<ActiveReviewer> ActiveReviewers { get { return this.context.ActiveReviewers.ToList<ActiveReviewer>(); } }
        public ActiveReviewerRepository(AppDBContext context)
        {
            this.context = context;
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
