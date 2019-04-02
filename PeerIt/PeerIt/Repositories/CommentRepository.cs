using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeerIt.Models;
using PeerIt.Interfaces;
namespace PeerIt.Repositories
{
    public class CommentRepository  : IGenericRepository<Comment, int>
    {
        AppDBContext context;

        public List<Comment> Comments { get { return this.context.Comments.ToList<Comment>(); } }
        public CommentRepository(AppDBContext context)
        {
            this.context = context;
        }

        public Comment FindByID(int ID)
        {
            foreach(Comment comment in this.Comments)
            {
                if (comment.ID == ID)
                    return comment;
            }
            return null;
        }

        public List<Comment> GetAll()
        {
            return this.Comments;
        }

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

        public Comment Add(Comment model)
        {
            try
            {
                Comments.Add(model);
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
