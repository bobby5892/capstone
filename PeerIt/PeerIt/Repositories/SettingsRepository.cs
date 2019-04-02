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
            this.Settings.each( (setting) => {
                if(setting.ID = ID)
                {
                    return setting;
                }
            });
            return null;
        }

        public List<Setting> GetAll()
        {
            return Settings;
        }

        public bool Edit(Setting model)
        {
         try { 
               Setting setting = FindByID(model.ID);
                // Need Replace
                if (context.SaveChanges() > 0)
                {
                    return true;
                }
            }
            catch(Exception e) {
                return false;
            }
            return false;
        }

        public bool Delete(Setting model)
        {
            throw new NotImplementedException();
        }

        public Setting Add(Setting model)
        {
            if (model.IsValid())
            {
                context.Settings.Add(model);
                context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
