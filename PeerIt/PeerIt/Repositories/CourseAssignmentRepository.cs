using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeerIt.Models;
using PeerIt.Interfaces;
namespace PeerIt.Repositories
{
    public class CourseAssignmentRepository : IGenericRepository<CourseAssignment, int>
    {
        AppDBContext context;

        public List<CourseAssignment> CourseAssignments { get { return this.context.CourseAssignments.ToList<CourseAssignment>(); } }
        public CourseAssignmentRepository(AppDBContext context)
        {
            this.context = context;
        }

        public CourseAssignment FindByID(int ID)
        {
            throw new NotImplementedException();
        }

        public List<CourseAssignment> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool Edit(CourseAssignment model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(CourseAssignment model)
        {
            throw new NotImplementedException();
        }

        public CourseAssignment Add(CourseAssignment model)
        {
            throw new NotImplementedException();
        }
    }
}
