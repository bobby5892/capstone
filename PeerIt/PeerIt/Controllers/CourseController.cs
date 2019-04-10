using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using PeerIt.Models;
using PeerIt.Infrastructure;
using PeerIt.ViewModels;
using PeerIt.Interfaces;
namespace PeerIt.Controllers
{
    public class CourseController : Controller
    {
        private UserManager<AppUser> usrMgr;
        public IGenericRepository<Course, int> courseRepository;
        public IGenericRepository<CourseGroup, int> courseGroupRepository;
        public CourseController(IGenericRepository<Course, int> courseRepository,
            IGenericRepository<CourseGroup, int> courseGroupRepository,
            UserManager<AppUser> usrMgr)
        {
            this.courseRepository = courseRepository;
            this.courseGroupRepository = courseGroupRepository;
            this.usrMgr = usrMgr;
        }
        /// <summary>
        /// Get a List of courses
        /// </summary>
        /// <returns></returns>
        /// 


        [HttpGet]
        // An Instructor is only going to see courses they are teaching
        // A student is only going to see courses they are in
        // an admin is going to see all courses
        public JsonResult getCourses()
        {
            JsonResponse<Course> response = new JsonResponse<Course>();
            var result = courseRepository.GetAll();
            if (result == null)
            {
                response.Error.Add(new Error() { Name = "Course", Description = "No Course found" });
                return Json(response);
            }
            else
            {
                courseRepository.GetAll().ForEach(course =>
                {
                    response.Data.Add(course);
                });
                bool isAdmin = HttpContext.User.IsInRole("admin");
                bool isStudent = HttpContext.User.IsInRole("student");
                bool isTeacher = HttpContext.User.IsInRole("instuctor");
                //var roles = await usrMgr.GetRolesAsync(user); 
                return Json(response);
            }
        }
        [HttpGet]
        [Authorize]//[any user]
        public JsonResult getCourse(int courseID)
        {
            JsonResponse<Course> response = new JsonResponse<Course>();
            var result = courseRepository.FindByID(courseID);
            if (result == null)
            {
                response.Error.Add(new Error() { Name = "Course", Description = "No Course found" });
                return Json(response);
            }
            else
            {
                response.Data.Add(result);
                return Json(response);
            }
        }
        [HttpGet]
        //[instructor, admin]
        public JsonResult getStudents(int courseID)
        {
            JsonResponse<Course> response = new JsonResponse<Course>();
            JsonResponse<AppUser> responseAU = new JsonResponse<AppUser>();
            var result = courseRepository.FindByID(courseID);
            var resultG = courseGroupRepository.GetAll();

            //what if there are no courses
            if (result == null)
            {
                response.Error.Add(new Error() { Name = "Course", Description = "No Course found" });
                return Json(response);
            }
            else
            {
                resultG.ForEach(group =>
                {
                    responseAU.Data.Add(group.FK_AppUser);
                });
            }
            if (responseAU == null)
            {
                response.Error.Add(new Error() { Name = "Students", Description = "No Students found" });
                return Json(response);
            }
            else
            {
                return Json(responseAU);
            }
            //return null;
        }
        [HttpGet]
        //[instructor, admin]
        public JsonResult getStudentsUngraded(int courseID)
        {
            return null;
        }
        [HttpGet]
        //[any user]
        public JsonResult getCoursesByUser(string userID)
        {
            return null;
        }


        /*GET +(LIST<COURSE>) getCourses() [anyUser] - Change results depending on role
        GET +(COURSE) getCourse(int courseID) [anyUser]
        GET + (LIST<APP_USER>) getStudents(int courseID) [instructor, admin]
        GET + (LIST<APP_USER>) getStudentsUngraded(int courseID) [instructor, admin]
        GET + (LIST<COURSE>) getCoursesByUser(string userID) [any user]
        
        
        
        PATCH +(bool) toggleEnabled(int courseID) [instructor, admin]
        PUT +(bool) setInstructor(int courseID, string userId)  [admin]
        PUT +(bool) setName(int courseID, string name) [admin]
        POST + (bool) createCourse(string courseName) [admin, instructor]
        DELETE + (bool) deleteCourse(int courseID, string courseName) [admin, instructor]*/

    }
}
