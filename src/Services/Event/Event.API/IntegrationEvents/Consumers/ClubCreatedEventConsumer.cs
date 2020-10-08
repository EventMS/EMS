using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using EMS.Event_Services.API.Context;
using EMS.Event_Services.API.Context.Model;
using EMS.Events;
using MassTransit;
using Serilog;

namespace EMS.Event_Services.API.Events
{
    public class ClubCreatedEventConsumer :
            IConsumer<ClubCreatedEvent>
    {
        private readonly EventContext _context;

        public ClubCreatedEventConsumer(EventContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<ClubCreatedEvent> context)
        {
            var club = _context.Clubs.Find(context.Message.ClubId);
            if (club == null)
            {
                _context.Clubs.Add(new Club()
                {
                    ClubId = context.Message.ClubId
                });
                await _context.SaveChangesAsync();
            }
        }
    }
}
