using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeerIt.Models;

namespace PeerIt.Repositories
{
    public class ActiveReviewerRepository
    {
        AppDBContext context;

        public List<ActiveReviewer> ActiveReviewers { get { return this.context.ActiveReviewers.ToList<ActiveReviewer>(); } }
        public ActiveReviewerRepository(AppDBContext context)
        {
            this.context = context;
        }
    }
}
