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
            throw new NotImplementedException();
        }

        public List<Comment> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool Edit(Comment model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Comment model)
        {
            throw new NotImplementedException();
        }

        public Comment Add(Comment model)
        {
            throw new NotImplementedException();
        }
    }
}
