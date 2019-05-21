using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using PeerIt.Models;

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

        private bool isAdmin;
        private bool isInstructor;
        private bool isStudent;

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
        [Authorize(Roles = "Administrator,Instructor,Student")]
        public async Task<JsonResult> GetCourses()
        {
            SetRoles();
            JsonResponse<CourseDataOut> response = new JsonResponse<CourseDataOut>();
            if (this.isAdmin)
            {
                // Show all courses
                List<CourseDataOut> courseDataOuts = new List<CourseDataOut>();
                this.courseRepository.GetAll().ToList<Course>().ForEach(course =>
                {
                    CourseDataOut dataOut = new CourseDataOut
                    {
                        ID = course.ID,
                        Name = course.Name,
                        IsActive = course.IsActive,
                        FK_INSTRUCTOR_NAME = course.FK_INSTRUCTOR.FirstName + " " + course.FK_INSTRUCTOR.LastName
                    };
                    courseDataOuts.Add(dataOut);
                });
                response.Data = courseDataOuts;
            }
            else if (this.isInstructor)
            {
                var currentUser = await usrMgr.GetUserAsync(HttpContext.User);
                // Lookup the course the instructor is teaching
                this.courseRepository.GetAll().ToList<Course>()
                    .ForEach(course =>
                    {
                        if (course.FK_INSTRUCTOR.Id == currentUser.Id)
                        {
                            CourseDataOut dataOut = new CourseDataOut
                            {
                                ID = course.ID,
                                Name = course.Name,
                                IsActive = course.IsActive,
                              //  FK_INSTRUCTOR_NAME = course.FK_INSTRUCTOR.FirstName + " " + course.FK_INSTRUCTOR.LastName
                            };
                            response.Data.Add(dataOut);
                        }
                    });
            }
            else if (this.isStudent)
            {
                List<CourseDataOut> courseDataOuts = new List<CourseDataOut>();
                AppUser currentUser = await usrMgr.GetUserAsync(HttpContext.User);
                // Look at Course Groups and add courses that the student is in
                this.courseGroupRepository.GetAll().ToList<CourseGroup>().ForEach(
                    courseGroup =>
                    {
                        if (courseGroup.FK_AppUser == currentUser)
                        {
                            //AppUser instructor = courseGroup.FK_Course.FK_INSTRUCTOR;
                            courseDataOuts.Add(new CourseDataOut
                            {
                                ID = courseGroup.FK_Course.ID,
                                Name = courseGroup.FK_Course.Name,
                                IsActive = courseGroup.FK_Course.IsActive,
                              //  FK_INSTRUCTOR_NAME = instructor.FirstName + " " + instructor.LastName
                            });
                        }
                    });
                response.Data = courseDataOuts;
            }
            else
            {
                response.Error.Add(new Error() { Name = "Courses", Description = "Your in a role that cannot view courses" });
            }
            return  Json(response);
        }

        void SetRoles()
        {
            this.isAdmin = HttpContext.User.IsInRole("Administrator");
            this.isInstructor = HttpContext.User.IsInRole("Instructor");
            this.isStudent = HttpContext.User.IsInRole("Student");
        }

        /// <summary>
        /// getCourse
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Administrator,Instructor,Student")]
        public async Task<JsonResult> GetCourse(int courseID = -1)
        {
            SetRoles();
            JsonResponse<Course> response = new JsonResponse<Course>();
            JsonResponse<CourseDataOut> responseStudent = new JsonResponse<CourseDataOut>();

            if (this.isAdmin)
            {
                //If they are admin we don't need to check if they are in the course
                response.Data.Add(this.courseRepository.FindByID(courseID));
            }
            else if (isInstructor)
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
            else if (this.isStudent)
            {
                //Lets see if the student is in the course]
               AppUser curentUser = await usrMgr.GetUserAsync(HttpContext.User);

               Course course = this.courseGroupRepository.GetAll().ToList<CourseGroup>().Find(
                    // Look for the user
                     x =>
                        {
                            if ((x.FK_AppUser ==  curentUser) && 
                                    (x.FK_Course == this.courseRepository.FindByID(courseID)))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                ).FK_Course;

                AppUser instructor = course.FK_INSTRUCTOR;
                CourseDataOut dataOut = new CourseDataOut
                {
                    ID = course.ID,
                    Name = course.Name,
                    IsActive = course.IsActive,
                    FK_INSTRUCTOR_NAME = instructor.FirstName + " " + instructor.LastName
                };

                if (responseStudent.Data.Count > 0)
                {
                    responseStudent.Data.Add(dataOut);
                    return Json(responseStudent);
                }
            }
            // If we didn't find the course show error
            if(response.Data.Count == 0)
            {
                response.Error.Add(new Error() { Name = "GetCourse", Description = "You have not been added to any courses" });
            }
            return Json(response);
        }
        [HttpGet]
        [Authorize(Roles = "Administrator,Instructor")]
        public async Task<JsonResult> GetStudents(int courseID)
        {
            SetRoles();
            JsonResponse<AppUser> response = new JsonResponse<AppUser>();
            bool authorizedToSee = false;

            if (this.isAdmin)
            {
                // This is a list of courseGroup - for that course / Could be optimized by adding a repo method
                authorizedToSee = true;
               
            }
            else if (this.isInstructor)
            {
                //Lookup the course
                Course courseLookup = this.courseRepository.FindByID(courseID);
                if(courseLookup.FK_INSTRUCTOR == await usrMgr.GetUserAsync(HttpContext.User))
                {
                    // this instructor is teaching the course
                    authorizedToSee = true;
                }
            }
            // Now if they are authorized lets do it
            if (authorizedToSee)
            {
                List<CourseGroup> courseGroups;
                // Go thru course groups - and grab all that are related to the course
                courseGroups = this.courseGroupRepository.GetAll().FindAll(x =>
                {
                    if (x.FK_Course.ID == courseID)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                });
                // Add the Students from the courseGroups to the response
                courseGroups.ForEach(x => response.Data.Add(x.FK_AppUser));
            }
            return Json(response);
        }
        [HttpGet]
        [Authorize(Roles = "Administrator,Instructor")]
        //[instructor, admin]
        public JsonResult GetStudentsUngraded(int courseID)
        {
            throw new NotImplementedException();
        }
        [HttpGet]
        [Authorize(Roles = "Administrator,Instructor,Student")]
        public async Task<JsonResult> GetCoursesByUser(string userID)
        {
            SetRoles();
            JsonResponse<Course> response = new JsonResponse<Course>();
            JsonResponse<CourseDataOut> responseStudent = new JsonResponse<CourseDataOut>();
            AppUser currentUser = await usrMgr.GetUserAsync(HttpContext.User);

            // Depending on who they are changes what is expected
            if (isAdmin)
            {
                // All courses
                response.Data = this.courseRepository.GetAll();
            }
            else if(isInstructor){
                // Get all the courses they are teaching
                response.Data = this.courseRepository.GetAll().FindAll(x => {
                    if(x.FK_INSTRUCTOR == currentUser)
                    {
                        return true;
                    }
                    else {
                        return false;
                    }
                });
            }
            else if (isStudent)
            {
                // Get all courses that they are in - this could be optimized by adding a repo method
                List<CourseGroup> courseGroups;
                courseGroups = this.courseGroupRepository.GetAll().FindAll(x =>
                {
                    if (x.FK_AppUser == currentUser)
                    {
                        return true;
                    }
                    else { return true; }
                });
                // Add the courses to response
                courseGroups.ForEach(courseGroup => responseStudent.Data.Add(
                    new CourseDataOut
                    {
                        ID = courseGroup.FK_Course.ID,
                        Name = courseGroup.FK_Course.Name,
                        IsActive = courseGroup.FK_Course.IsActive,
                        FK_INSTRUCTOR_NAME = courseGroup.FK_Course.FK_INSTRUCTOR.FirstName + " " +
                            courseGroup.FK_Course.FK_INSTRUCTOR.LastName
                    }
                ));
            }
            return Json(response);
        }
        [Authorize(Roles = "Administrator,Instructor")]
        [HttpPatch]
        public async Task<JsonResult> ToggleEnabled(int courseID)
        {
            SetRoles();
            JsonResponse<bool> response = new JsonResponse<bool>();
            bool actionApproved = false;
            Course courseToChange = this.courseRepository.FindByID(courseID);
            // If course not found
            if (courseToChange == null)
            {
                response.Error.Add(new Error() { Name = "course", Description = "Course not found" });
                return Json(response);
            }
            // They are admin - ok
            if (this.isAdmin) { actionApproved = true; }
            // Check to see if its their course
            else if ((this.isInstructor) && (courseToChange.FK_INSTRUCTOR == await usrMgr.GetUserAsync(HttpContext.User)))
            {
                 actionApproved = true;
            }
            // Lets do it if the have access
            if (actionApproved)
            {
                // Flip it
                courseToChange.IsActive = !courseToChange.IsActive;
                this.courseRepository.Edit(courseToChange);
            }
            else
            {
                response.Error.Add(new Error() { Name = "course", Description = "Access Denied" });
            }
            return Json(response);
        }

        [HttpPut]
        [Authorize(Roles = "Administrator")]
        async Task<JsonResult> SetInstructor(int courseID, string userId) {
            SetRoles();
            JsonResponse<Course> response = new JsonResponse<Course>();
            Course lookupCourse = this.courseRepository.FindByID(courseID);

            AppUser newInstructor = await usrMgr.FindByIdAsync(userId);
            // Does the course exist
            if (lookupCourse == null)
            {
                response.Error.Add(new Error() { Name = "course", Description = "No such course" });
                return Json(response);
            }
            // Does the User Exist
            if(newInstructor == null)
            {
                response.Error.Add(new Error() { Name = "course", Description = " No such instructor" });
                return Json(response);
            }
            // Ready to swap it
            // Change instructor
            lookupCourse.FK_INSTRUCTOR = newInstructor;
            // save it
            this.courseRepository.Edit(lookupCourse);
            // add the course to response and print it
            response.Data.Add(lookupCourse);
            return Json(response);
        }
        [HttpPut]
        [Authorize(Roles = "Administrator")]
        JsonResult SetName(int courseID, string name)
        {
            SetRoles();
            JsonResponse<Course> response = new JsonResponse<Course>();
            Course lookupCourse = this.courseRepository.FindByID(courseID);
            // Check if course exists
            if (lookupCourse == null)
            {
                response.Error.Add(new Error() { Description="Course does not exist",Name="course"});
                return Json(response);
            }
            //Change the name
            lookupCourse.Name = name;
            response.Data.Add(lookupCourse);
            //save it
            this.courseRepository.Edit(lookupCourse);
            return Json(response);
        }
        [Authorize(Roles = "Administrator,Instructor")]
        [HttpPost]
        public async Task<JsonResult> CreateCourse(string courseName)
        {
            JsonResponse<Course> response = new JsonResponse<Course>();
            SetRoles();
            Course newCourse = new Course() {Name=courseName,FK_INSTRUCTOR = await usrMgr.GetUserAsync(HttpContext.User) };
            newCourse = this.courseRepository.Add(newCourse);
            if (newCourse == null)
            {
                response.Error.Add(new Error() { Description = "Course not created", Name = "CourseController" });
            }
            else
            {
                response.Data.Add(newCourse);
            }
            return Json(response);
        }
        /// <summary>
        /// Delete a course [admin/instructor]
        /// </summary>
        /// <param name="courseID"></param>
        /// <param name="courseName"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator,Instructor")]
        [HttpPost]
        async Task<JsonResult>  DeleteCourse(int courseID, string courseName)
        {
            SetRoles();
            JsonResponse<Course> response = new JsonResponse<Course>();
            Course lookupCourse = this.courseRepository.FindByID(courseID);
            if (lookupCourse == null)
            {
                response.Error.Add(new Error() {Name="deleteCourse",Description="Course Not Found" });
                return Json(response);
            }

            if (this.isAdmin)
            {
                if (courseName == lookupCourse.Name)
                {
                    this.courseRepository.Delete(lookupCourse);
                    return Json(response);
                }
            }
            else if (this.isInstructor)
            {
                // Check if instructor is teaching the course
                if(
                    (lookupCourse.FK_INSTRUCTOR == await usrMgr.GetUserAsync(HttpContext.User)) && (
                    courseName == lookupCourse.Name)){
                    this.courseRepository.Delete(lookupCourse);
                    return Json(response);
                }
            }
            response.Error.Add(new Error() { Name = "courseDelete", Description = "Course Name confirmation delete failed" });
            return Json(response);
        }
        [Authorize(Roles = "Administrator,Instructor")]
        [HttpPost]
        public async Task<JsonResult> AddStudentToCourse(int courseID, string studentEmail) {
            SetRoles();
            JsonResponse<CourseGroup> response = new JsonResponse<CourseGroup>();
            Course lookupCourse = this.courseRepository.FindByID(courseID);
            AppUser student = await usrMgr.FindByEmailAsync(studentEmail);
            if(student == null)
            {
                response.Error.Add(new Error() { Name = "addStudentToCourse", Description = "Student doesn't exist" });
                return Json(response);
            }
            if (lookupCourse == null)
            {
                response.Error.Add(new Error() { Name = "addStudentToCourse", Description = "Course Not Found" });
                return Json(response);
            }
            // Lets see if this student is already in the course
            bool alreadyEnrolled = this.courseGroupRepository.GetAll().Exists((x) => {
                if(x.FK_Course.ID == courseID && x.FK_AppUser.Id == student.Id)
                {
                    return true;
                }
                return false;
            });
            if (alreadyEnrolled)
            {
                response.Error.Add(new Error() { Name = "addStudentToCourse", Description = "Student is already in class" });
                return Json(response);
            }
            var newCourseGroup = new CourseGroup() { FK_AppUser = student, FK_Course = lookupCourse, ReviewGroup = "1" };
            if (this.isAdmin)
            {
                this.courseGroupRepository.Add(newCourseGroup);
                response.Data.Add(newCourseGroup);
                return Json(response);
            }
            else if (this.isInstructor)
            {
                // Check if instructor is teaching the course
                if 
                    (lookupCourse.FK_INSTRUCTOR == await usrMgr.GetUserAsync(HttpContext.User))
                {
                    this.courseGroupRepository.Add(newCourseGroup);
                    response.Data.Add(newCourseGroup);
                    return Json(response);
                }
            }
            response.Error.Add(new Error() { Name = "studentAdd", Description = "failed to add student to course" });
            return Json(response);
        }
        [Authorize(Roles = "Administrator,Instructor")]
        [HttpDelete]
        public async Task<JsonResult> RemoveStudentToCourse(int courseID, string studentID)
        {
            SetRoles();
            JsonResponse<CourseGroup> response = new JsonResponse<CourseGroup>();
            Course lookupCourse = this.courseRepository.FindByID(courseID);
            AppUser student = await usrMgr.FindByIdAsync(studentID);
            if (student == null)
            {
                response.Error.Add(new Error() { Name = "RemoveStudentToCourse", Description = "Student doesn't exist" });
                return Json(response);
            }
            if (lookupCourse == null)
            {
                response.Error.Add(new Error() { Name = "addStudentToCourse", Description = "Course Not Found" });
                return Json(response);
            }
            // lets find the enrollment
            var enrollment = this.courseGroupRepository.GetAll().FindAll((x) => {
                if(x.FK_Course.ID == courseID && x.FK_AppUser.Id == studentID)
                {
                    return true;
                }
                return false;
            });
            if (enrollment.Count == 0)
            {
                response.Error.Add(new Error() { Name = "RemoveStudentToCourse", Description = "Student is not in the class" });
                return Json(response);
            }
            if (this.isAdmin)
            {
                enrollment.ForEach((x) => {
                    this.courseGroupRepository.Delete(x);
                });
                return Json(response);
            }
            else if (this.isInstructor)
            {
                // Check if instructor is teaching the course
                if
                    (lookupCourse.FK_INSTRUCTOR == await usrMgr.GetUserAsync(HttpContext.User))
                {
                    enrollment.ForEach((x) => {
                        this.courseGroupRepository.Delete(x);
                    });
                    return Json(response);
                }
            }
            response.Error.Add(new Error() { Name = "RemoveStudentToCourse", Description = "failed to remove student to course" });
            return Json(response);
        }
        [Authorize(Roles = "Administrator,Instructor")]
        [HttpPatch]
        public async Task<JsonResult> ChangeStudentGroup(string studentID, int courseID, string reviewGroupID)
        {
            JsonResponse<CourseGroup> response = new JsonResponse<CourseGroup>();
            AppUser currentUser = await usrMgr.GetUserAsync(HttpContext.User);
            CourseGroup courseGroup = null;
            courseGroupRepository.GetAll().ForEach(cG => 
            {
                if (cG.FK_Course.ID == courseID && cG.FK_AppUser.Id == studentID)
                {
                    courseGroup = cG;
                }
            });
            SetRoles();
            if (courseGroup != null)
            {
                if (this.isAdmin || this.isInstructor && courseGroup.FK_Course.FK_INSTRUCTOR.Id == currentUser.Id)
                {
                    courseGroup.ReviewGroup = reviewGroupID;
                    if (courseGroupRepository.Edit(courseGroup))
                    {
                        response.Data.Add(courseGroup);
                    }
                    else
                    {
                        response.Error.Add(new Error("NotSuccessful", "The data was not successfully writen."));
                    }
                }
                else
                {
                    response.Error.Add(new Error("Forbidden", "You are not allowed here."));
                }
            }
            else
            {
                response.Error.Add(new Error("NotFound", "The Course Group was not found."));
            }
            return Json(response);
        }
    }
}