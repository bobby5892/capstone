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
            throw new NotImplementedException();
        }

        public List<Event> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool Edit(Event model)
        {
            throw new NotImplementedException();
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
