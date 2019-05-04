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

        public JsonResult GetSettings()
        {
            JsonResponse<Setting> response = new JsonResponse<Setting>();
            List<Setting> settings = settingRepository.GetAll();
            if (settings != null)
            {
                response.Data = settings;
                return Json(response);
            }
            response.Error.Add(new Error("NotFound", "Could not access Config Settings."));
            return Json(response);
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
        public JsonResult EditSetting (string Id, string stringValue, int numericValue)
        {
            JsonResponse<Setting> response = new JsonResponse<Setting>();
            var result = settingRepository.FindByID(Id);
            if (result != null)
            {
                Setting newSet = new Setting
                {
                    ID = Id,
                    StringValue = stringValue,
                    NumericValue = numericValue
                };
                if (settingRepository.Edit(newSet))
                {
                    response.Data.Add(newSet);
                    return Json(response);
                }
            }
            return null;
        }
        public JsonResult EditSettings(string isEnabledId, int isEnabledValue,
                                       string serverId, string serverValue,
                                       string portId, int portValue,
                                       string usernameId, string usernameValue,
                                       string passwordId, string passwordValue)
        {
            JsonResponse<Setting> response = new JsonResponse<Setting>();
            Setting enabledSetting = settingRepository.FindByID(isEnabledId);
            Setting serverSetting = settingRepository.FindByID(serverId);
            Setting portSetting = settingRepository.FindByID(portId);
            Setting usernameSetting = settingRepository.FindByID(usernameId);
            Setting passwordSetting = settingRepository.FindByID(passwordId);

            if (enabledSetting != null && serverSetting != null &&
                portSetting != null && usernameId != null &&
                passwordSetting != null)
            {
                usernameSetting.StringValue = usernameValue;
                usernameSetting.NumericValue = 0;
                if (settingRepository.Edit(usernameSetting))
                {
                    response.Data.Add(usernameSetting);
                }
                else
                {
                    response.Error.Add(new Error("NotSuccessful", usernameId + " did not successfully write"));
                }
                portSetting.NumericValue = portValue;
                portSetting.StringValue = null;
                if (settingRepository.Edit(portSetting))
                {
                    response.Data.Add(portSetting);
                }
                else
                {
                    response.Error.Add(new Error("NotSuccessful", portId + " did not successfully write"));
                }
                serverSetting.StringValue = serverValue;
                serverSetting.NumericValue = 0;
                if (settingRepository.Edit(serverSetting))
                {
                    response.Data.Add(serverSetting);
                }
                else
                {
                    response.Error.Add(new Error("NotSuccessful", serverId + " did not successfully write"));
                }
                enabledSetting.NumericValue = isEnabledValue;
                enabledSetting.StringValue = null;
                if (settingRepository.Edit(enabledSetting))
                {
                    response.Data.Add(enabledSetting);
                }
                else
                {
                    response.Error.Add(new Error("NotSuccessful", isEnabledId + " did not successfully write"));
                }
                passwordSetting.StringValue = passwordValue;
                passwordSetting.NumericValue = 0;
                if (settingRepository.Edit(passwordSetting))
                {
                    response.Data.Add(passwordSetting);
                }
                else
                {
                    response.Error.Add(new Error("NotSuccessful", passwordId + " did not successfully write"));
                }
            }
            else
            {
                response.Error.Add(new Error("NotFound", "Data was not found."));
            }
            return Json(response);
        }
    }
}
