using System.Linq;
using EMS.EventVerification_Services.API.Context;
using EMS.EventVerification_Services.API.Context.Model;

namespace EMS.EventVerification_Services.API.GraphQlQueries
{
    public class EventVerificationQueries
    {
        private readonly EventVerificationContext _context;
        public EventVerificationQueries(EventVerificationContext context)
        {
            _context = context;
        }

        public IQueryable<EventVerification> EventVerifications => _context.EventVerifications.AsQueryable();
    }
}
