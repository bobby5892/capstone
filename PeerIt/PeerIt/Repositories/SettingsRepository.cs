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
        /// <summary>
        /// List of Settings
        /// </summary>
        public List<Setting> Settings { get { return this.context.Settings.ToList<Setting>(); } }
        /// <summary>
        /// Overloaded Constructor that accepts an AppDBContext
        /// </summary>
        /// <param name="context"></param>
        public SettingsRepository(AppDBContext context)
        {
            this.context = context;
        }
        /// <summary>
        /// Find a setting by ID
        /// </summary>
        /// <param name="ID">string</param>
        /// <returns></returns>
        public Setting FindByID(string ID)
        {
            Setting result = null; 
            this.Settings.ForEach( (setting) => {
                if(setting.ID == ID)
                {
                    result= setting;
                }
            });
            return result ;
        }
        /// <summary>
        ///  Get a list of all settings
        /// </summary>
        /// <returns></returns>
        public List<Setting> GetAll()
        {
            return Settings;
        }
        /// <summary>
        /// Edit a Setting
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
                e = null;
                return false;
            }
            return false;
        }
        /// <summary>
        /// Delete a setting
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Delete(Setting model)
        {

            Setting setting = FindByID(model.ID);
            if (setting != null)
            {
                Settings.Remove(setting);
                return true;
            }
            return false;
        }
        /// <summary>
        /// Add a setting
        /// </summary>
        /// <param name="model">Setting</param>
        /// <returns>Setting</returns>
        public Setting Add(Setting model)
        {
            context.Settings.Add(model);
            context.SaveChanges();
            return model;
        }
    }
}
