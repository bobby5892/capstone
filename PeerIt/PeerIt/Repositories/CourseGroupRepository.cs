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
            CourseGroup cGroup = from g in context.CourseGroups
                                 where g.ID = ID
                                 select g;

            return cGroup;
        }

        public List<CourseGroup> GetAll()
        {
            return CourseGroups;
        }

        public bool Edit(CourseGroup model)
        {
            var cGroup = FindByID(model.ID).SingleOrDefault();

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

        public bool Delete(CourseGroup model)
        {
            var cGroup = FindByID(model.Id).SingleOrDefault();

            if (cGroup != null)
            {
                context.Delete(cGroup);
                if (context.SaveChanges() > 0)
                {
                    return true;
                }
            }
            return false;
        }

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
