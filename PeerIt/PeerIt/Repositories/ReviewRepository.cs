using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeerIt.Interfaces;
using PeerIt.Models;
namespace PeerIt.Repositories
{
    public class ReviewRepository : IGenericRepository<Review, int>
    {
        AppDBContext context;

        public List<Review> Reviews { get { return this.context.Reviews.ToList<Review>(); } }
        public ReviewRepository(AppDBContext context)
        {
            this.context = context;
        }

        public Review FindByID(int ID)
        {
            throw new NotImplementedException();
        }

        public List<Review> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool Edit(Review model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Review model)
        {
            throw new NotImplementedException();
        }

        public Review Add(Review model)
        {
            throw new NotImplementedException();
        }
    }
}
