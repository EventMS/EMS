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
            var room = _context.Rooms.Find(context.Message.RoomId);
            if (room == null)
            {
                /*if (_context.Clubs.Find(context.Message.ClubId) == null)
                {
                    _context.Clubs.Add(new Club()
                    {
                        ClubId = context.Message.ClubId
                    });
                }*/
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