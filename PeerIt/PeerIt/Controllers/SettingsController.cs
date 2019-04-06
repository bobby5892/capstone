using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.AspNetCore.Mvc;
using PeerIt.Models;
using PeerIt.ViewModels;
using PeerIt.Interfaces;
namespace PeerIt.Controllers
{
    public class SettingsController : Controller
    {
        public IGenericRepository<Setting, string> settingRepository;
        public SettingsController(IGenericRepository<Setting, string> settingRepository)
        {
            this.settingRepository = settingRepository;
        }
        /// <summary>
        /// Retrieve a specific setting by looking up the ID
        /// </summary>
        /// <param name="id">string</param>
        /// <returns></returns>
        public JsonResult GetSetting (string id)
        {
            // Get ready to respond with JSON
            JsonResponse<Setting> response = new JsonResponse<Setting>();
            // Lookup the setting by ID
            var result = settingRepository.FindByID(id);
            // If we didn't find anything - build and show an error
            if(result == null)
            {
                // Add an error to the list (there can be more than one)
                response.Error.Add(new Error() { Name = "Settings", Description = "No setting found by that ID" });
                //Stop running code we're done - return back.
                return Json(response);
            }
            //Add a Piece of Data - in this case <setting> to the list
            response.Data.Add(result);
            // send it back to the browser
            return Json(response);
        }
        public JsonResult EditSetting (string ID, string StringValue, int NumericValue)
        {
            return null;
        }
        public JsonResult GetSettings()
        {
            return null;
        }
    }
}
