using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeerIt.Interfaces;
using PeerIt.Models;
namespace PeerIt.Repositories
{
    public class EventRepository : IGenericRepository<Event, int>
    {
        AppDBContext context;

        public List<Event> Events { get { return this.context.Events.ToList<Event>(); } }

        /// <summary>
        /// Overloaded Constructor
        /// </summary>
        /// <param name="context"></param>
        public EventRepository(AppDBContext context)
        {
            this.context = context;
        }
        /// <summary>
        /// Returns an Event by it's ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns name="Event"></returns>
        public Event FindByID(int ID)
        {
            // get event by event id out of list

            foreach (Event e in Events)
            {
                if (e.ID == ID)
                {
                    return e;
                }
            }
            return null;
        }
        /// <summary>
        /// Returns all Event Objects in the dbcontext
        /// </summary>
        /// 
        public List<Event> GetAll()
        {
            //throw new NotImplementedException();
            // return list
            return Events;
        }
        /// <summary>
        /// Edits an Event in the dbcontext, and returns a bool indicating
        /// if it is successful
        /// </summary>
        /// <param name="model">Event</param>
        /// <returns name="bool">bool</returns>
        public bool Edit(Event model)
        {
            Event temp = FindByID(model.ID);

            if (temp != null)
            {
                context.Events.Update(model);
                context.SaveChanges();
                return true;
            }
            return false;
        }
        /// <summary>
        /// Deletes an event from the dbcontext, and returns a bool indicating
        /// if it is successful
        /// </summary>
        /// <param name="model">Event</param>
        /// <returns>bool</returns>
        public bool Delete(Event model)
        {
            Event temp = FindByID(model.ID);
            if (temp != null)
            {
                context.Events.Remove(model);
                context.SaveChanges();
                return true;
            }
            return false;
        }
        /// <summary>
        /// Adds an event to the dbcontext, and returns a copy of the event
        /// if it is successful.
        /// </summary>
        /// <param name="model">Event</param>
        /// <returns>Event</returns>
        public Event Add(Event model)
        {
            Event temp = FindByID(model.ID);
            if (temp == null)
            {
                context.Events.Add(model);
                context.SaveChanges();
                return model;
            }
            else
            {
                return model;
            }
        }
    }
}
