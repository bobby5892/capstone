using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeerIt.Interfaces;
using PeerIt.Models;
namespace PeerIt.Repositories
{
    public class InvitationRepository : IGenericRepository<Invitation, int>
    {
        AppDBContext context;

        public List<Invitation> Invitations { get { return this.context.Invitations.ToList<Invitation>(); } }
        public InvitationRepository(AppDBContext context)
        {
            this.context = context;
        }

        public Invitation FindByID(int ID)
        {
            throw new NotImplementedException();
        }

        public List<Invitation> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool Edit(Invitation model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Invitation model)
        {
            throw new NotImplementedException();
        }

        public Invitation Add(Invitation model)
        {
            throw new NotImplementedException();
        }
    }
}
