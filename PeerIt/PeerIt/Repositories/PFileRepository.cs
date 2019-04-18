using PeerIt.Interfaces;
using PeerIt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeerIt.Repositories
{
    ///
    public class PFileRepository : IGenericRepository<PFile, int>
    {
        AppDBContext context;
        ///
        public List<PFile> Files { get { return context.PFiles.ToList<PFile>(); } }

        ///
        public PFile Add(PFile model)
        {
            context.PFiles.Add(model);
            if (context.SaveChanges() > 0)
            {
                return model;
            }
            return null;
        }

        ///
        public bool Delete(PFile model)
        {
            throw new NotImplementedException();
        }

        ///
        public bool Edit(PFile model)
        {
            throw new NotImplementedException();
        }

        ///
        public PFile FindByID(int ID)
        {
            foreach (PFile file in this.Files)
            {
                //if (file.ID == ID)
                //    return file;
            }
            return null;
        }

        ///
        public List<PFile> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
