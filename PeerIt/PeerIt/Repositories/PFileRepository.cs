﻿using PeerIt.Interfaces;
using PeerIt.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeerIt.Repositories
{
    ///
    public class PFileRepository : IGenericRepository<PFile, string>
    {
        AppDBContext context;
        ///
        public PFileRepository(AppDBContext context)
        {
            this.context = context;
        }
        ///
        public List<PFile> PFiles { get { return context.PFiles.ToList<PFile>(); } }

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

        ///
        public bool Edit(PFile model)
        {
            throw new NotImplementedException();
        }

        ///
        public PFile FindByID(string ID)
        {
            foreach (PFile file in this.PFiles)
            {
                if (file.ID == ID)
                    return file;
            }
            return null;
        }

        ///
        public List<PFile> GetAll()
        {
            return this.PFiles;
        }
    }
}
