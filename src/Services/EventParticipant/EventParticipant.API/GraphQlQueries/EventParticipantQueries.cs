using System.Linq;
using EMS.Club_Service_Services.API;
using EMS.EventParticipant_Services.API.Context;
using EMS.EventParticipant_Services.API.Context.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

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

        public IQueryable<EventParticipant> MyEventParticipations([CurrentUserGlobalState] CurrentUser user)
        {
            return _context.EventParticipants.Where(evPart => evPart.UserId == user.UserId);
        }
    }
}
