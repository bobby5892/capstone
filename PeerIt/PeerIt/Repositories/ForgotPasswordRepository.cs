using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeerIt.Interfaces;
using PeerIt.Models;

namespace PeerIt.Repositories
{
    public class ForgotPasswordRepository : IGenericRepository<ForgotPassword, int>
    {
        AppDBContext context;

        /// <summary>
        /// Returns all ForgotPassword objects in the dbcontext.
        /// </summary>
        /// <returns></returns>
        public List<ForgotPassword> ForgotPasswords { get { return this.context.ForgotPasswords.ToList<ForgotPassword>(); } }

        /// <summary>
        /// Overloaded Constructor
        /// </summary>
        /// <returns></returns>
        public ForgotPasswordRepository(AppDBContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Finds a ForgotPassword in the dbcontext by the ID
        /// </summary>
        /// <returns></returns>
        public ForgotPassword FindByID(int ID)
        {
            ForgotPassword result = null;
            this.ForgotPasswords.ForEach((forgotPassword) => {
                if (forgotPassword.ID == ID)
                {
                    result = forgotPassword;
                }
            });
            return result;
        }

        /// <summary>
        /// Returns all the ForgotPasswords in the dbcontext.
        /// </summary>
        /// <returns></returns>
        public List<ForgotPassword> GetAll()
        {
            return ForgotPasswords;
        }

        /// <summary>
        /// Edits abstract ForgotPassword in the dbcontext and returns a bool
        /// indicating if it is successful.
        /// </summary>
        /// <returns></returns>
        public bool Edit(ForgotPassword model)
        {
            ForgotPassword forgotPassword = FindByID(model.ID);
            if (forgotPassword != null)
            {
                forgotPassword = model;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Deletes a ForgotPassword from the dbcontext, and returns a bool
        /// indicating if it is successful.
        /// </summary>
        /// <returns></returns>
        public bool Delete(ForgotPassword model)
        {
            ForgotPassword temp = FindByID(model.ID);
            if (temp != null)
            {
                context.ForgotPasswords.Remove(model);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds a ForgotPassword to the dbcontext, and returns a copy if it
        /// is successful.
        /// </summary>
        /// <returns></returns>
        public ForgotPassword Add(ForgotPassword model)
        {
            try
            {
                ForgotPasswords.Add(model);
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
