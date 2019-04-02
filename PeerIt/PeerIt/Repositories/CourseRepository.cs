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

        public List<Course> Courses { get { return this.context.Course.ToList<Course>(); } }
        public CourseRepository(AppDBContext context)
        {
            this.context = context;
        }

        public Course FindByID(int ID)
        {
            Course course = from c in context.Course
                            where c.ID == ID
                            select c;

            return course;
        }

        public List<Course> GetAll()
        {
            return this.Courses;
        }

        public bool Edit(Course model)
        {
            Course course = FindByID(model.ID);

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

        public bool Delete(Course model)
        {
            var course = FindByID(model.ID).SingleOrDefault();

            if (course != null)
            {
                context.Remove(course);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public Course Add(Course model)
        {
            try
            {
                context.Course.Add(model);
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
