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
            foreach (Invitation invitation in this.Invitations)
            {
                if (invitation.ID == ID)
                    return invitation;
            }
            return null;
        }

        public List<Invitation> GetAll()
        {
            return this.Invitations;
        }

        public bool Edit(Invitation model)
        {
            Invitation invitation = FindByID(model.ID);
            if (invitation != null)
            {
                invitation = model;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool Delete(Invitation model)
        {
            Invitation invitation = FindByID(model.ID);
            if (invitation != null)
            {
                Invitations.Remove(invitation);
                return true;
            }
            return false;
        }

        public Invitation Add(Invitation model)
        {
            try
            {
                Invitations.Add(model);
                context.SaveChanges();
                return Invitations[Invitations.Count - 1];
            }
            catch
            {
                return null;
            }
        }
    }
}
