using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeerIt.Interfaces;
using PeerIt.Models;
namespace PeerIt.Repositories
{
    public class CourseRepository : IGenericRepository<Course, int>
    {
        AppDBContext context;

        public List<Course> Courses { get { return this.context.Courses.ToList<Course>(); } }
        /// <summary>
        /// Overloaded Constructor for passing a context to Course Repository
        /// </summary>
        /// <param name="context"></param>
        public CourseRepository(AppDBContext context)
        {
            this.context = context;
        }
        /// <summary>
        /// Find Course by ID
        /// </summary>
        /// <param name="ID">int</param>
        /// <returns></returns>
        public Course FindByID(int ID)
        {
            Course result = null;
            this.Courses.ForEach((course) => {
                if (course.ID == ID)
                {
                    result = course;
                }
            });
            return result;
        }
        /// <summary>
        /// Get all courses
        /// </summary>
        /// <returns></returns>
        public List<Course> GetAll()
        {
            return this.Courses;
        }
        /// <summary>
        /// Edit a course
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Edit(Course model)
        {
            var course = FindByID(model.ID);

            if (course != null)
            {
                course = model;
                if (context.SaveChanges() > 0)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        ///  Delete a course
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Delete(Course model)
        {
            var course = FindByID(model.ID);

            if (course != null)
            {
                context.Courses.Remove(course);
                if (context.SaveChanges() > 0)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        ///  Add a Course
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Course Add(Course model)
        {
            try
            {
                context.Courses.Add(model);
                context.SaveChanges();
                return FindByID(model.ID);
            }
            catch
            {
                return null;
            }
        }
    }
}
