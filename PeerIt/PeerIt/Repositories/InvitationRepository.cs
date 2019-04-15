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

        /// <summary>
        /// Returns all the Invitations from the dbcontext.
        /// </summary>
        /// <returns></returns>
        public List<Invitation> Invitations { get { return this.context.Invitations.ToList<Invitation>(); } }


        /// <summary>
        /// Overloaded Constructor
        /// </summary>
        /// <returns></returns>
        public InvitationRepository(AppDBContext context)
        {
            this.context = context;
        }
        
        /// <summary>
        /// Finds an Invitation by it's ID in the dbcontext.
        /// </summary>
        /// <returns></returns>
        public Invitation FindByID(int ID)
        {
            foreach (Invitation invitation in this.Invitations)
            {
                if (invitation.ID == ID)
                    return invitation;
            }
            return null;
        }

        /// <summary>
        /// Returns all Invitations in the dbcontext.
        /// </summary>
        /// <returns></returns>
        public List<Invitation> GetAll()
        {
            return this.Invitations;
        }

        /// <summary>
        /// Edits an Invitation in the dbcontext, and returns a bool indicating
        /// if it is successful
        /// </summary>
        /// <returns></returns>
        public bool Edit(Invitation model)
        {
            Invitation invitation = FindByID(model.ID);
            if (invitation != null)
            {
                invitation = model;
                if (context.SaveChanges() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Deletes an Invitation in the dbcontext, and returns a bool
        /// indicating if it is successful.
        /// </summary>
        /// <returns></returns>
        public bool Delete(Invitation model)
        {
            Invitation invitation = FindByID(model.ID);
            if (invitation != null)
            {
                Invitations.Remove(invitation);
                if (context.SaveChanges() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Adds an Invitation to the dbcontext, and returns abstract copy if
        /// it is successful.
        /// </summary>
        /// <returns></returns>
        public Invitation Add(Invitation model)
        {
            try
            {
                Invitations.Add(model);
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
