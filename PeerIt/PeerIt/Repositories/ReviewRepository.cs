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
            foreach (Review review in this.Reviews)
            {
                if (review.ID == ID)
                    return review;
            }
            return null;
        }

        public List<Review> GetAll()
        {
            return this.Reviews;
        }

        public bool Edit(Review model)
        {
            Review review = FindByID(model.ID);
            if (review != null)
            {
                review = model;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool Delete(Review model)
        {
            Review review = FindByID(model.ID);
            if (review != null)
            {
                Reviews.Remove(review);
                return true;
            }
            return false;
        }

        public Review Add(Review model)
        {
            try
            {
                Reviews.Add(model);
                context.SaveChanges();
                return Reviews[Reviews.Count - 1];
            }
            catch
            {
                return null;
            }
        }
    }
}
