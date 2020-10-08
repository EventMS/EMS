using System.Threading.Tasks;
using EMS.Event_Services.API.Context;
using EMS.Event_Services.API.Context.Model;
using EMS.Events;
using MassTransit;

namespace EMS.Event_Services.API.Events
{
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

    public class TimeslotReservedConsumer :
        IConsumer<TimeslotReserved>
    {
        private readonly EventContext _context;

        public TimeslotReservedConsumer(EventContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<TimeslotReserved> context)
        {
            var @event = _context.Events.Find(context.Message.EventId);
            if (@event == null)
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