using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeerIt.Models;
using PeerIt.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace PeerIt.Repositories
{
    public class CommentRepository  : IGenericRepository<Comment, int>
    {
        AppDBContext context;
        /// <summary>
        /// Returns List of Comments
        /// </summary>
        //public List<Comment> Comments { get { return this.context.Comments.ToList<Comment>(); } }
        public List<Comment> Comments
        {
            get
            {
                return this.context.Comments
                    .Include(comments => comments.FK_STUDENT_ASSIGNMENT.CourseAssignment.FK_COURSE.FK_INSTRUCTOR)
                    .Include(comments => comments.FK_STUDENT_ASSIGNMENT.AppUser)
                    .Include(comments => comments.FK_APP_USER)
                    .ToList();
            }
        }
        /// <summary>
        /// Overloaded Constructor that accepts an AppDBContext
        /// </summary>
        /// <param name="context"></param>
        public CommentRepository(AppDBContext context)
        {
            this.context = context;
        }
        /// <summary>
        /// Find a Comment by ID
        /// </summary>
        /// <param name="ID">int</param>
        /// <returns></returns>
        public Comment FindByID(int ID)
        {
            foreach(Comment comment in this.Comments)
            {
                if (comment.ID == ID)
                    return comment;
            }
            return null;
        }
        /// <summary>
        /// Get a list of all Comments
        /// </summary>
        /// <returns></returns>
        public List<Comment> GetAll()
        {
            return this.Comments;
        }
        /// <summary>
        /// Edit a comment
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Edit(Comment model)
        {
            Comment comment = FindByID(model.ID);
            if(comment != null)
            {
                comment = model;
                context.SaveChanges();
                return true;
            }
            return false;
        }
        /// <summary>
        /// Delete a comment
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Delete(Comment model)
        {
            Comment comment = FindByID(model.ID);
            if(comment != null)
            {
                Comments.Remove(comment);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Add a comment
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Comment Add(Comment model)
        {
            try
            {
                context.Comments.Add(model);
                context.SaveChanges();
                return Comments[Comments.Count - 1];
            }
            catch
            {
                return null;
            }
        }
    }
}
