using System.Threading.Tasks;
using EMS.Event_Services.API.Context;
using EMS.Event_Services.API.Context.Model;
using EMS.Events;
using MassTransit;

namespace EMS.Event_Services.API.Events
{
    public class ClubSubscriptionCreatedEventConsumer :
        IConsumer<ClubSubscriptionCreatedEvent>
    {
        private readonly EventContext _context;

        public ClubSubscriptionCreatedEventConsumer(EventContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<ClubSubscriptionCreatedEvent> context)
        {
            var subscription = _context.Subscriptions.Find(context.Message.ClubId);
            if (subscription == null)
            {
                _context.Subscriptions.Add(new Subscription()
                {
                    SubscriptionId = context.Message.SubscriptionId,
                    ClubId = context.Message.ClubId
                });
                await _context.SaveChangesAsync();
            }
        }
    }

    public class RoomCreatedEventConsumer :
        IConsumer<RoomCreatedEvent>
    {
        private readonly EventContext _context;

        public RoomCreatedEventConsumer(EventContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<RoomCreatedEvent> context)
        {
            var subscription = _context.Rooms.Find(context.Message.ClubId);
            if (subscription == null)
            {
                _context.Rooms.Add(new Room()
                {
                    ClubId = context.Message.ClubId,
                    RoomId = context.Message.RoomId
                });
                await _context.SaveChangesAsync();
            }
        }
    }
}