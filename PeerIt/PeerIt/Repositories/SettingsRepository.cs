using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeerIt.Models;
using PeerIt.Interfaces;
namespace PeerIt.Repositories
{
    public class SettingsRepository : IGenericRepository<Setting, string>
    {
        AppDBContext context;

        public List<Setting> Settings { get { return this.context.Settings.ToList<Setting>(); } }
        public SettingsRepository(AppDBContext context)
        {
            this.context = context;
        }

        public Setting FindByID(string ID)
        {
            throw new NotImplementedException();
        }

        public List<Setting> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool Edit(Setting model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Setting model)
        {
            throw new NotImplementedException();
        }

        public Setting Add(Setting model)
        {
            throw new NotImplementedException();
        }
    }
}
