using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using PeerIt.Models;
using PeerIt.Repositories;
using PeerIt.ViewModels;
using PeerIt.Interfaces;

namespace PeerIt.Controllers
{
    /// <summary>
    /// A controller that handles User Feed updates and events.
    /// </summary>
    public class EventController : Controller
    {
        #region Constructors

        private UserManager<AppUser> userManager;
        private IGenericRepository<Event,int> eventRepository;

        /// <summary>
        /// OverLoaded Constructor
        /// </summary>
        /// <param name="userMgr"></param>
        /// <param name="eventRepo"></param>
        public EventController(UserManager<AppUser> userMgr,
            IGenericRepository<Event, int> eventRepository)
        {
            userManager = userMgr;
            this.eventRepository = eventRepository;
            
        }

        #endregion Constructors

        #region Methods that return Json

        /// <summary>
        /// Retrieves Events by the AppUser's ID.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        /// 

        [HttpPost]
        public async Task<JsonResult> GetEventsByUser(string userID)
        {
            JsonResponse<Event> response = new JsonResponse<Event>();
            AppUser user = await userManager.FindByIdAsync(userID);
            //string userID = user.Id;

            if (user != null)
            {
                // response.Data.Add(eventRepository.GetByUserID(user.Id));
               
                response.Data = eventRepository.GetAll().FindAll(x => {
                    if (x.FK_AppUser.Id == user.Id)
                    {
                        return true;
                    }
                    return false;
                });
            }
            else
            {
                response.Error.Add(new Error("NotFound", "User was not Found."));
            }

            return Json(response);
        }
        [HttpGet]
        public async Task<JsonResult> GetEventsByUser()
        {
            JsonResponse<Event> response = new JsonResponse<Event>();
            AppUser user = await userManager.GetUserAsync(HttpContext.User);
            //string userID = user.Id;

            if (user != null)
            {

                response.Data = eventRepository.GetAll().FindAll(x => {
                    if (x.FK_AppUser.Id == user.Id)
                    {
                        return true;
                    }
                    return false;
                });

            }
            else
            {
                response.Error.Add(new Error("NotFound", "User was not Found."));
            }

            return Json(response);
        }

        /// <summary>
        /// Gets an event by the ID, and toggles HasSeen to true;
        /// </summary>
        /// <param name="eventID"></param>
        /// <returns></returns>
        public JsonResult GetEventByID(int eventID)
        {
            JsonResponse<Event> response = new JsonResponse<Event>();
            Event requestedEvent = eventRepository.FindByID(eventID);

            /*if (requestedEvent != null)
            {
                if (eventRepository.ToggleHasSeen(eventID))
                {
                    response.Data.Add(requestedEvent);
                    return Json(response);
                }
            }*/
            response.Error.Add(new Error("NotFound", "Event was not Found"));
            return Json(response);
        }

        #endregion Methods that return Json
    }
}
