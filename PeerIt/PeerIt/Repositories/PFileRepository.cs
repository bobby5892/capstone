using PeerIt.Interfaces;
using PeerIt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeerIt.Repositories
{
    /// <summary>
    /// PFileRepository
    /// </summary>
    public class PFileRepository : IGenericRepository<PFile, string>
    {
        AppDBContext context;
        /// <summary>
        /// Contstructor takes the database context 
        /// </summary>
        /// <param name="context"></param>
        public PFileRepository(AppDBContext context)
        {
            this.context = context;
        }
        /// <summary>
        /// List of type PFile
        /// </summary>
        public List<PFile> PFiles { get { return context.PFiles.ToList<PFile>(); } }

        /// <summary>
        /// Add a PFile to the database
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public PFile Add(PFile model)
        {
            context.PFiles.Add(model);
            if (context.SaveChanges() > 0)
            {
                return model;
            }
            return null;
        }

        /// <summary>
        /// Delete 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Delete(PFile model)
        {
            PFile file = FindByID(model.ID);
            
            if(file.ID != null)
            {
                this.PFiles.Remove(file);
                if(context.SaveChanges() < 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Edit
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Edit(PFile model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Find by Id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public PFile FindByID(string ID)
        {
            foreach (PFile file in this.PFiles)
            {
                if (file.ID == ID)
                    return file;
            }
            return null;
        }

        /// <summary>
        /// Get em all
        /// </summary>
        /// <returns></returns>
        public List<PFile> GetAll()
        {
            return this.PFiles;
        }
    }
}
