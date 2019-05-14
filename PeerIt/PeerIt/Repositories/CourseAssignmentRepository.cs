using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeerIt.Models;
using PeerIt.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace PeerIt.Repositories
{
    public class CourseAssignmentRepository : IGenericRepository<CourseAssignment, int>
    {
        AppDBContext context;

        /// <summary>
        /// Returns all of the CourseAssignment objects in the dbcontext
        /// </summary>
        /// <returns></returns>
        //public List<CourseAssignment> CourseAssignments { get { return this.context.CourseAssignments.ToList<CourseAssignment>(); } }
        public List<CourseAssignment> CourseAssignments
        {
            get
            {
                return context.CourseAssignments
                    .Include(cAssignments => cAssignments.FK_COURSE.FK_INSTRUCTOR)
                    .Include(cAssignments => cAssignments.PFile)
                    .ToList();
            }
        }
        public CourseAssignmentRepository(AppDBContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Finds a CouseAssignment object by it's ID
        /// </summary>
        /// <returns></returns>
        public CourseAssignment FindByID(int ID)
        {
            foreach (CourseAssignment courseAssignment in this.CourseAssignments)
            {
                if (courseAssignment.ID == ID)
                    return courseAssignment;
            }
            return null;
        }

        /// <summary>
        /// Returns all of the CourseAssignments in the dbcontext
        /// </summary>
        /// <returns></returns>
        public List<CourseAssignment> GetAll()
        {
            return this.CourseAssignments;
        }

        /// <summary>
        /// Gets all Assignments by a courseID
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public List<CourseAssignment> GetByCourseID(int courseID)
        {
            List<CourseAssignment> courseAssignments = new List<CourseAssignment>();

            foreach (CourseAssignment cA in this.CourseAssignments)
            {
                if (cA.FK_COURSE.ID == courseID)
                {
                    courseAssignments.Add(cA);
                }
            }
            return courseAssignments;
        }

        /// <summary>
        /// Edits a CourseAssignment in the dbcontext and returns a bool
        /// indicating if it is successful.
        /// </summary>
        /// <returns></returns>
        public bool Edit(CourseAssignment model)
        {
            var cAssign = FindByID(model.ID);

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

        /// <summary>
        /// Deletes a CourseAssignment from he dbcontext, and returns a bool
        /// indicating if it is successful
        /// </summary>
        /// <returns></returns>
        public bool Delete(CourseAssignment model)
        {
            var cAssign = FindByID(model.ID);

            if (cAssign != null)
            {
                context.CourseAssignments.Remove(cAssign);
                if (context.SaveChanges() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Adds a CourseAssignment to the dbcontext, and returns a copy
        /// if it is successful.
        /// </summary>
        /// <returns></returns>
        public CourseAssignment Add(CourseAssignment model)
        {
            try
            {
                context.CourseAssignments.Add(model);
                context.SaveChanges();
                return model;
            }
            catch
            {
                return null;
            }
        }
    }
}
