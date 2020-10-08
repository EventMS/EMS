using System.Linq;
using EMS.Event_Services.API.Context;
using EMS.Event_Services.API.Context.Model;

namespace EMS.Event_Services.API.GraphQlQueries
{
    public class EventQueries
    {
        private readonly EventContext _context;
        public EventQueries(EventContext context)
        {
            _context = context;
        }

        public IQueryable<Event> Events => _context.Events.AsQueryable();
    }
}
