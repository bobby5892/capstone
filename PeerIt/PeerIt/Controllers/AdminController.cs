using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PeerIt.Controllers
{
    public class AdminController : Controller
    {
        #region Private Repository Variables

        #endregion Private Repository Variables

        #region Constructors

        public AdminController() 
        {

        }

        #endregion Constructors

        #region Methods that return Json

        [HttpGet]
        public JsonResult GetSettings() 
        {
            return Json(false);
        }

        public JsonResult CreateInstructor() 
        {
            return null;
        }

        #endregion Methods that return Json
    }
}
