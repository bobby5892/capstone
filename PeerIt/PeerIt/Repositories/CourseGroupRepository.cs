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
        /// Gets a list of CourseGroups by a user's ID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<CourseGroup> GetByUserID(string userID)
        {
            List<CourseGroup> classes = new List<CourseGroup>();
            this.CourseGroups.ForEach(courseGroup =>
            {
                if (courseGroup.FK_AppUser.Id == userID)
                {
                    classes.Add(courseGroup);
                }
            });
            return classes;
        }

        /// <summary>
        /// Gets a list of CourseGroups by a course's ID.
        /// </summary>
        /// <param name="courseID"></param>
        /// <returns></returns>
        public List<CourseGroup> GetByCourseID(int courseID)
        {
            List<CourseGroup> classes = new List<CourseGroup>();
            this.CourseGroups.ForEach(courseGroup =>
            {
                if (courseGroup.FK_Course.ID == courseID)
                {
                    classes.Add(courseGroup);
                }
            });
            return classes;
        }

        public CourseGroup GetByUserAndCourseID(string userID, int courseID)
        {
            CourseGroup courseGroup = null;
            this.CourseGroups.ForEach(cg =>
            {
                if (cg.FK_Course.ID == courseID && cg.FK_AppUser.Id == userID)
                {
                    courseGroup = cg;
                }
            });
            return courseGroup;
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
