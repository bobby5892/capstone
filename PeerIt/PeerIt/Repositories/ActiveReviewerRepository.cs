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
        /// <summary>
        /// List of ActiveReviewers
        /// </summary>
        public List<ActiveReviewer> ActiveReviewers { get { return this.context.ActiveReviewers.ToList<ActiveReviewer>(); } }
        /// <summary>
        /// Constructor that accepts the Database Context
        /// </summary>
        /// <param name="context"></param>
        public ActiveReviewerRepository(AppDBContext context)
        {
            this.context = context;
        }
        /// <summary>
        /// Find ActiveReviewer By ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActiveReviewer FindByID(int ID)
        {
            ActiveReviewer result = null;
            this.ActiveReviewers.ForEach((activeViewer) => {
                if (activeViewer.ID == ID)
                {
                    result = activeViewer;
                }
            });
            return result;
        }
        /// <summary>
        /// Get all Active Reviewer
        /// </summary>
        /// <returns></returns>
        public List<ActiveReviewer> GetAll()
        {
            return ActiveReviewers;
        }

        /// <summary>
        /// Edits an ActiveReviewer, and returns a bool indicating if it is
        /// successful.
        /// </summary>
        /// <returns></returns>
        public bool Edit(ActiveReviewer model)
        {
            ActiveReviewer activeReviewer = FindByID(model.ID);
            if (activeReviewer != null)
            {
                activeReviewer = model;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Deletes an ActiveReviewer object from the dbcontext, and returns
        /// a bool indicating if it is successful.
        /// </summary>
        /// <returns></returns>
        public bool Delete(ActiveReviewer model)
        {
            var aReviewer = FindByID(model.ID);

            if (aReviewer != null) 
            {
                context.ActiveReviewers.Remove(model);
                if (context.SaveChanges() > 0) 
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Adds an ActiveReviewer to the dbcontext, and returns a copy
        /// if it is successful.
        /// </summary>
        /// <returns></returns>
        public ActiveReviewer Add(ActiveReviewer model)
        {
            try 
            {
                context.ActiveReviewers.Add(model);
                context.SaveChanges();
                return model;
            }
            catch 
            {
                return null;
            }
        }
    }
}
