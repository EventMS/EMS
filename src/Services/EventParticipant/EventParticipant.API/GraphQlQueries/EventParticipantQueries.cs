using System.Linq;
using EMS.EventParticipant_Services.API.Context;
using EMS.EventParticipant_Services.API.Context.Model;

namespace EMS.EventParticipant_Services.API.GraphQlQueries
{
    public class EventParticipantQueries
    {
        private readonly EventParticipantContext _context;
        public EventParticipantQueries(EventParticipantContext context)
        {
            _context = context;
        }

        public IQueryable<EventParticipant> EventParticipants => _context.EventParticipants.AsQueryable();
    }
}
