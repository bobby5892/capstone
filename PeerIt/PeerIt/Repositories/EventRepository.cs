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
        public EventRepository(AppDBContext context)
        {
            this.context = context;
        }

        public Event FindByID(int ID)
        {
            // get event by event id out of list

            foreach(Event e in Events)
            {
                if (e.ID == ID)
                {
                    return e;
                }
                else
                {
                    return null;
                }
            }

            //throw new NotImplementedException();
        }

        public List<Event> GetAll()
        {
            //throw new NotImplementedException();
            // return list
            return Events;

        }

        public bool Edit(Event model)
        {
            //throw new NotImplementedException();
            Event temp = FindByID(model.ID);

            if (temp != null)
            {
                temp = model;
                context.SaveChanges();
                return true;
            }
            else
                false;

        }

        public bool Delete(Event model)
        {
            throw new NotImplementedException();
        }

        public Event Add(Event model)
        {
            throw new NotImplementedException();
        }
    }
}
