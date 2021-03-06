﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeerIt.Models;
using PeerIt.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace PeerIt.Repositories
{
    /// <summary>
    /// Student assignment Repository
    /// </summary>
    public class StudentAssignmentRepository : IGenericRepository<StudentAssignment, int>
    {
        AppDBContext context;

        /// <summary>
        /// Returns all of the CourseAssignment objects in the dbcontext
        /// </summary>
        /// <returns></returns>
        //public List<StudentAssignment> StudentAssignments { get { return this.context.StudentAssignments.ToList<StudentAssignment>(); } }
        public List<StudentAssignment> StudentAssignments
        {
            get
            {
                return this.context.StudentAssignments
                    .Include(sAssignments => sAssignments.CourseAssignment.FK_COURSE.FK_INSTRUCTOR)
                    .Include(sAssignments => sAssignments.AppUser)
                     .Include(sAssignments => sAssignments.FK_PFile)
                    .ToList();
            }
        }
        public StudentAssignmentRepository(AppDBContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Finds a CouseAssignment object by it's ID
        /// </summary>
        /// <returns></returns>
        public StudentAssignment FindByID(int ID)
        {
            foreach (StudentAssignment studentAssignment in this.StudentAssignments)
            {
                if (studentAssignment.ID == ID)
                    return studentAssignment;
            }
            return null;
        }

        /// <summary>
        /// Returns all of the CourseAssignments in the dbcontext
        /// </summary>
        /// <returns></returns>
        public List<StudentAssignment> GetAll()
        {
            return this.StudentAssignments;
        }
        /// <summary>
        /// Edits a CourseAssignment in the dbcontext and returns a bool
        /// indicating if it is successful.
        /// </summary>
        /// <returns></returns>
        public bool Edit(StudentAssignment model)
        {
            var sAssign = FindByID(model.ID);

            if (sAssign != null)
            {
                sAssign = model;
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
        public bool Delete(StudentAssignment model)
        {
            var sAssign = FindByID(model.ID);

            if (sAssign != null)
            {
                context.StudentAssignments.Remove(sAssign);
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
        public StudentAssignment Add(StudentAssignment model)
        {
            try
            {
                context.StudentAssignments.Add(model);
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
