using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeerIt.Interfaces;
using PeerIt.Models;
using Microsoft.EntityFrameworkCore;
namespace PeerIt.Repositories
{
    /// <summary>
    ///Review Repository Class
    /// </summary>
    public class ReviewRepository : IGenericRepository<Review, int>
    {
        AppDBContext context;
        /// <summary>
        /// List of Reviews
        /// </summary>
        //public List<Review> Reviews { get { return this.context.Reviews.ToList<Review>(); } }
        public List<Review> Reviews
        {
            get
            {
                return this.context.Reviews
                    .Include(reviews => reviews.FK_STUDENT_ASSIGNMENT.CourseAssignment.FK_COURSE.FK_INSTRUCTOR)
                    .Include(reviews => reviews.FK_STUDENT_ASSIGNMENT.AppUser)
                    .Include(reviews => reviews.FK_APP_USER)
                    .ToList();
            }
        }
        /// <summary>
        /// Overloaded Constructor 
        /// </summary>
        /// <param name="context"></param>
        public ReviewRepository(AppDBContext context)
        {
            this.context = context;
        }
        /// <summary>
        /// Returns a Review by ID
        /// </summary>
        /// <param name="ID">int</param>
        /// <returns></returns>
        public Review FindByID(int ID)
        {
            foreach (Review review in this.Reviews)
            {
                if (review.ID == ID)
                    return review;
            }
            return null;
        }
        /// <summary>
        ///  Return a list of Reviews
        /// </summary>
        /// <returns></returns>
        public List<Review> GetAll()
        {
            return this.Reviews;
        }
        /// <summary>
        /// Gets a list of Reviews by a Student Assignment's ID
        /// </summary>
        /// <param name="assignmentID"></param>
        /// <returns></returns>
        public List<Review> GetReviewsByAssignment(int assignmentID)
        {
            List<Review> reviews = new List<Review>();
            Reviews.ForEach(r =>
            {
                if (r.FK_STUDENT_ASSIGNMENT.ID == assignmentID)
                {
                    reviews.Add(r);
                }
            });
            return reviews;
        }
        /// <summary>
        /// Edit a review
        /// </summary>
        /// <param name="model">Review</param>
        /// <returns></returns>
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
        /// <summary>
        /// Delete a review
        /// </summary>
        /// <param name="model">Review</param>
        /// <returns>bool - success/fail</returns>
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
        /// <summary>
        /// Add a review
        /// </summary>
        /// <param name="model">Review</param>
        /// <returns>Review</returns>
        public Review Add(Review model)
        {
            context.Reviews.Add(model);
            if (context.SaveChanges() > 0)
            {
                return model;
            }
            return null;
        }
    }
}
