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

        /// <summary>
        /// Overloaded Constructor
        /// </summary>
        /// <returns></returns>
        public CourseGroupRepository(AppDBContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Finds abstract CourseGroup Object by an ID
        /// </summary>
        /// <returns></returns>
        public CourseGroup FindByID(int ID)
        {
            CourseGroup result = null;
            this.CourseGroups.ForEach((courseGroup) => {
                if (courseGroup.ID == ID)
                {
                    result = courseGroup;
                }
            });
            return result;
        }

        /// <summary>
        /// Returns all CourseGroup objects in the dbcontext
        /// </summary>
        /// <returns></returns>
        public List<CourseGroup> GetAll()
        {
            return CourseGroups;
        }

        /// <summary>
        /// Edits a CourseGroup object in the dbcontext, and returns a copy
        /// if it is successful
        /// </summary>
        /// <returns></returns>
        public bool Edit(CourseGroup model)
        {
            var cGroup = FindByID(model.ID);

            if (cGroup != null)
            {
                cGroup = model;
                if (context.SaveChanges() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Deletes a CourseGroup object from the dbcontext, and returns a
        /// copy if it is successful
        /// </summary>
        /// <returns></returns>
        public bool Delete(CourseGroup model)
        {
            var cGroup = FindByID(model.ID);

            if (cGroup != null)
            {
                context.CourseGroups.Remove(cGroup);
                if (context.SaveChanges() > 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Adds a CourseGroup to the dbcontext, and returns a copy if it is
        /// successful
        /// </summary>
        /// <returns></returns>
        public CourseGroup Add(CourseGroup model)
        {
            try
            {
                context.CourseGroups.Add(model);
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
