using System;
using System.Linq;
using System.Threading.Tasks;
using EMS.Event_Services.API.Context;
using EMS.Event_Services.API.Context.Model;
using Microsoft.EntityFrameworkCore;

namespace EMS.Event_Services.API.GraphQlQueries
{
    public class EventQueries
    {
        private readonly EventContext _context;
        public EventQueries(EventContext context)
        {
            _context = context;
        }
        /*
        public IQueryable<Event> Events(EventStatus status) => _context.Events
            .Where(e => e.Status == status)
            .AsQueryable();
        */
        public IQueryable<Event> EventsConfirmed => _context.Events
           .Where(e => e.Status == EventStatus.Confirmed)
           .Include(e => e.Locations)
           .Include(e => e.EventPrices)
           .Include(e => e.InstructorForEvents).AsQueryable();
        public IQueryable<Event> Events => _context.Events
            .Include(e => e.Locations)
            .Include(e => e.EventPrices)
            .Include(e => e.InstructorForEvents).AsQueryable();

        public IQueryable<Event> EventsForClub(Guid clubId) => _context.Events
            .Where(e => e.ClubId == clubId && e.Status == EventStatus.Confirmed)
            .Include(e => e.Locations)
            .Include(e => e.EventPrices)
            .Include(e => e.InstructorForEvents)
            .AsQueryable();

        public async Task<Event> getEvent(Guid eventId) => await _context.Events.Include(e => e.Locations)
            .Include(e => e.EventPrices)
            .Include(e => e.InstructorForEvents).FirstOrDefaultAsync(e => e.EventId == eventId);
    }
}
