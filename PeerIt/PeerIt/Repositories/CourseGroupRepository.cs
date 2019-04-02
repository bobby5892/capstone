using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeerIt.Models;
using PeerIt.Interfaces;
namespace PeerIt.Repositories
{
    public class CourseGroupRepository : IGenericRepository<CourseGroup, int>
    {
        AppDBContext context;

        public List<CourseGroup> CourseGroups { get { return this.context.CourseGroups.ToList<CourseGroup>(); } }
        public CourseGroupRepository(AppDBContext context)
        {
            this.context = context;
        }

        public CourseGroup FindByID(int ID)
        {
            throw new NotImplementedException();
        }

        public List<CourseGroup> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool Edit(CourseGroup model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(CourseGroup model)
        {
            throw new NotImplementedException();
        }

        public CourseGroup Add(CourseGroup model)
        {
            throw new NotImplementedException();
        }
    }
}
