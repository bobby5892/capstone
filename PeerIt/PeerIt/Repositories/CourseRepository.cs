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
            throw new NotImplementedException();
        }

        public List<Course> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool Edit(Course model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Course model)
        {
            throw new NotImplementedException();
        }

        public Course Add(Course model)
        {
            throw new NotImplementedException();
        }
    }
}
