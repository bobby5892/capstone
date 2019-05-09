using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PeerIt.Models;
using PeerIt.ViewModels;
using PeerIt.Interfaces;
namespace PeerIt.Controllers
{
    public class SettingsController : Controller
    {
        public IGenericRepository<Setting, string> settingRepository;
        public UserManager<AppUser> userManager;
        /// <summary>
        /// This Controller deals with all requests regarding the Settings
        /// data and Email Configuration settings.
        /// </summary>
        /// <param name="settingRepository"></param>
        public SettingsController(IGenericRepository<Setting, string> settingRepository)
        {
            this.settingRepository = settingRepository;
        }

        /// <summary>
        /// Retrieves all Email Configuration settings in the database.
        /// There should only be 5 in total thus far.
        /// </summary>
        /// <returns></returns>
        public JsonResult GetSettings()
        {
            JsonResponse<Setting> response = new JsonResponse<Setting>();
            if (HttpContext.User.IsInRole("Administrator"))
            {
                List<Setting> settings = settingRepository.GetAll();
                if (settings != null)
                {
                    response.Data = settings;
                }
                else
                {
                    response.Error.Add(new Error("NotFound", "Could not access Config Settings."));
                }
            }
            else
            {
                response.Error.Add(new Error("Forbidden", "You are not allowed here."));
            }
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
            // Only send settings data to the Administrator.
            if (HttpContext.User.IsInRole("Administrator"))
            {
                // Lookup the setting by ID
                var result = settingRepository.FindByID(id);
                // check to see if the setting exists
                if (result != null)
                {
                    // if the setting exists, add the data to the response.
                    response.Data.Add(result);
                }
                else
                {
                    // If the setting doesn't exist, build and add an Error to the response.
                    response.Error.Add(new Error("Notfound", "The setting was not found."));
                }
            }
            else
            {
                // if the user isn't an Administrator, build and add an error to the response.
                response.Error.Add(new Error("Forbidden", "You are not allowed here."));
            }
            // send the response back to the browser
            return Json(response);
        }
        /// <summary>
        /// This method takes a setting data from a request, replaces it in 
        /// the database if valid, and returns the new setting data.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="stringValue"></param>
        /// <param name="numericValue"></param>
        /// <returns></returns>
        public JsonResult EditSetting (string Id, string stringValue, int numericValue)
        {
            JsonResponse<Setting> response = new JsonResponse<Setting>();
            if (HttpContext.User.IsInRole("Administrator"))
            {
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
                    }
                    else
                    {
                        response.Error.Add(new Error("NotSuccessful", "The data was not written successfully."));
                    }
                }
                else
                {
                    response.Error.Add(new Error("NotFound", "Setting was not found."));
                }
            }
            else
            {
                response.Error.Add(new Error("Forbidden", "You are not allowed here."));
            }
            return Json(response);
        }
        /// <summary>
        /// This method takes the full configuration data and replaces them.
        /// If valid, it then returns the data for the group of settings.
        /// </summary>
        /// <param name="isEnabledId"></param>
        /// <param name="isEnabledValue"></param>
        /// <param name="serverId"></param>
        /// <param name="serverValue"></param>
        /// <param name="portId"></param>
        /// <param name="portValue"></param>
        /// <param name="usernameId"></param>
        /// <param name="usernameValue"></param>
        /// <param name="passwordId"></param>
        /// <param name="passwordValue"></param>
        /// <returns></returns>
        public async Task<JsonResult> EditSettings(string isEnabledId, int isEnabledValue,
                                       string serverId, string serverValue,
                                       string portId, int portValue,
                                       string usernameId, string usernameValue,
                                       string passwordId, string passwordValue)
        {
            JsonResponse<Setting> response = new JsonResponse<Setting>();
            if (HttpContext.User.IsInRole("Administrator"))
            {
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
            }
            else
            {
                response.Error.Add(new Error("Forbidden", "You are not allowed here."));
            }
            return Json(response);
        }
    }
}
