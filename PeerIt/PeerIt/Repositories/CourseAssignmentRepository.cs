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
            CourseAssignment cAssign = from a in context.CourseAssignments
                                       where a.ID == ID
                                       select a;

            return cAssign;
        }

        public List<CourseAssignment> GetAll()
        {
            return this.CourseAssignments;
        }

        public bool Edit(CourseAssignment model)
        {
            var cAssign = FindByID(model.ID).SingleOrDefault();

            if (cAssign != null)
            {
                cAssign = model;
                if (context.SaveChanges() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool Delete(CourseAssignment model)
        {
            var cAssign = FindByID(model.ID).SingleOrDefault();

            if (cAssign != null)
            {
                context.Delete(cAssign);
                if (context.SaveChanges() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public CourseAssignment Add(CourseAssignment model)
        {
            try
            {
                context.CourseAssignments.Add(model);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return null;
            }
        }
    }
}
