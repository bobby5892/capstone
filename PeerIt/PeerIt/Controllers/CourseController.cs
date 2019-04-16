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
        private IGenericRepository<Course, int> courseRepository;
        private IGenericRepository<CourseGroup, int> courseGroupRepository;
        private IGenericRepository<CourseAssignment, int> courseAssignmentRepository;

        public CourseController(IGenericRepository<Course, int> courseRepository,
            IGenericRepository<CourseGroup, int> courseGroupRepository,
            IGenericRepository<CourseAssignment, int> courseAssignmentRepository,
            UserManager<AppUser> usrMgr)
        {
            this.courseRepository = courseRepository;
            this.courseGroupRepository = courseGroupRepository;
            this.courseAssignmentRepository = courseAssignmentRepository;
            this.usrMgr = usrMgr;
        }
        /// <summary>
        /// Get a List of courses
        /// </summary>
        /// <returns></returns>
        /// 


        [HttpGet]
        public async Task<JsonResult> GetCourses()
        {
            JsonResponse<Course> response = new JsonResponse<Course>();
            
            bool isAdmin = HttpContext.User.IsInRole("Administrator");
            bool isStudent = HttpContext.User.IsInRole("Student");
            bool isTeacher = HttpContext.User.IsInRole("Instructor");

            if (isAdmin)
            {
                // Show all courses
                response.Data = this.courseRepository.GetAll().ToList<Course>();

            }
            else if (isTeacher)
            {
                // Lookup the course the instructor is teaching
                this.courseRepository.GetAll().ToList<Course>()
                    .ForEach(async course =>
                    {
                        if (course.FK_INSTRUCTOR == await usrMgr.GetUserAsync(HttpContext.User))
                        {
                            response.Data.Add(course);
                        }
                    });
            }
            else if (isStudent)
            {
                // Look at Course Groups and add courses that the student is in
                this.courseGroupRepository.GetAll().ToList<CourseGroup>().ForEach(
                    async courseGroup =>
                    {
                        if (courseGroup.FK_AppUser == await usrMgr.GetUserAsync(HttpContext.User))
                        {
                            response.Data.Add(courseGroup.FK_Course);
                        }
                    });
            }
            else
            {
                response.Error.Add(new Error() { Name = "Courses", Description = "Your in a role that cannot view courses" });
            }
            return  Json(response);
        }
        /// <summary>
        /// getCourse
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<JsonResult> getCourse(int courseID)
        {
            JsonResponse<Course> response = new JsonResponse<Course>();
            bool isAdmin = HttpContext.User.IsInRole("Administrator");
            bool isStudent = HttpContext.User.IsInRole("Student");
            bool isTeacher = HttpContext.User.IsInRole("Instructor");

            if (isAdmin)
            {
                //If they are admin we don't need to check if they are in the course
                response.Data.Add(this.courseRepository.FindByID(courseID));
            }
            else if (isTeacher)
            {
                //Lookup the course
                Course courseLookup = this.courseRepository.FindByID(courseID);
                /// Lets see if that course exists
                if (courseLookup == null)
                {
                    response.Error.Add(new Error() { Description = "The course does not exist", Name = "Course" });
                    return Json(response);
                }
                //Lets see if the instructor is teacing the course
                if (courseLookup.FK_INSTRUCTOR == await usrMgr.GetUserAsync(HttpContext.User))
                {
                    response.Data.Add(courseLookup);
                }
            }
            else if (isStudent)
            {
                //Lets see if the student is in the course]
               AppUser curentUser = await usrMgr.GetUserAsync(HttpContext.User);

               response.Data.Add(this.courseGroupRepository.GetAll().ToList<CourseGroup>().Find(
                    // Look for the user
                     x =>
                        {
                            if ((x.FK_AppUser ==  curentUser) && 
                                    (x.FK_Course == this.courseRepository.FindByID(courseID))) {
                                return true;
                            }
                            else {
                                return false;
                            }
                        }
               ).FK_Course);
  
            }
            // If we didn't find the course show error
            if(response.Data.Count == 0)
            {
                response.Error.Add(new Error() { Name = "GetCourse", Description = "You have not been added to any courses" });
            }
            return Json(response);
            
        }
        [HttpGet]
        //[instructor, admin]
        public JsonResult getStudents(int courseID)
        {
            JsonResponse<AppUser> response = new JsonResponse<AppUser>();
            bool isAdmin = HttpContext.User.IsInRole("Administrator");
            bool isStudent = HttpContext.User.IsInRole("Student");
            bool isTeacher = HttpContext.User.IsInRole("Instructor");

            if (isAdmin)
            {
                // This is a list of courseGroup - for that course / Could be optimized by adding a repo method
             
                List<CourseGroup> courseGroups;
                // Go thru course groups - and grab all that are related to the course
                this.courseGroupRepository.GetAll().ForEach( x => {
                    if(x.FK_Course.ID == courseID)
                    {
                     // Continue Here   courseGroups.Add(x);
                    }
                });
             
            }

            /*   JsonResponse<Course> response = new JsonResponse<Course>();
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
               */
        }
        [HttpGet]
        //[instructor, admin]
        public JsonResult getStudentsUngraded(int courseID)
        {
            throw new NotImplementedException();
        }
        [HttpGet]
        //[any user]
        public async Task<JsonResult> getCoursesByUser(string userID)
        {
            AppUser user = await this.findAppUserbyID(userID);
            JsonResponse<Course> response = new JsonResponse<Course>();
            List<CourseGroup> courseGroups = courseGroupRepository.GetAll();
            courseGroups.ForEach(courseGroup =>
            {
                if (courseGroup.FK_AppUser == user){
                    response.Data.Add(courseGroup.FK_Course);
                }
            });

            return Json(response);
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
        private async Task<AppUser> findAppUserbyID(string userID)
        {
            AppUser user = await usrMgr.FindByIdAsync(userID);
            return user;

        }


    }
}