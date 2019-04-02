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

        public List<ForgotPassword> ForgotPasswords { get { return this.context.ForgotPasswords.ToList<ForgotPassword>(); } }
        public ForgotPasswordRepository(AppDBContext context)
        {
            this.context = context;
        }

        public ForgotPassword FindByID(int ID)
        {
            throw new NotImplementedException();
        }

        public List<ForgotPassword> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool Edit(ForgotPassword model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(ForgotPassword model)
        {
            throw new NotImplementedException();
        }

        public ForgotPassword Add(ForgotPassword model)
        {
            throw new NotImplementedException();
        }
    }
}
