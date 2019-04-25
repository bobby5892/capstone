using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using PeerIt.Models;
using PeerIt.ViewModels;
using PeerIt.Repositories;

namespace PeerIt.Controllers
{
    /// <summary>
    /// A controller that handles User Feed updates and events.
    /// </summary>
    public class EventController : Controller
    {
        #region Constructors

        private UserManager<AppUser> userManager;
        private EventRepository eventRepository;

        /// <summary>
        /// OverLoaded Constructor
        /// </summary>
        /// <param name="userMgr"></param>
        /// <param name="eventRepo"></param>
        public EventController(UserManager<AppUser> userMgr,
            EventRepository eventRepo)
        {
            userManager = userMgr;
            eventRepository = eventRepo;
        }

        #endregion Constructors

        #region Methods that return Json

        /// <summary>
        /// Retrieves Events by the AppUser's ID.
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> GetEventsByUser()
        {
            JsonResponse<List<Event>> response = new JsonResponse<List<Event>>();
            AppUser user = await userManager.GetUserAsync(HttpContext.User);
            string userID = user.Id;


            if (userID != null)
            {
                response.Data.Add(eventRepository.GetByUserID(userID));
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

            if (requestedEvent != null)
            {
                if (eventRepository.ToggleHasSeen(eventID))
                {
                    response.Data.Add(requestedEvent);
                    return Json(response);
                }
            }
            response.Error.Add(new Error("NotFound", "Event was not Found"));
            return Json(response);
        }

        #endregion Methods that return Json
    }
}
