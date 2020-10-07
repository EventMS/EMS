using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Events;
using EMS.Room_Services.API.Context;
using EMS.Room_Services.API.Context.Model;
using EMS.TemplateWebHost.Customization.EventService;
using MassTransit;
using Serilog;

namespace EMS.Room_Services.API.Events
{
    public class ClubCreatedEventConsumer :
            IConsumer<ClubCreatedEvent>
        {

            private readonly RoomContext _roomContext;
            private readonly IEventService _eventService;

            public ClubCreatedEventConsumer(RoomContext roomContext, IEventService eventService)
            {
                _roomContext = roomContext;
                _eventService = eventService;
            }

            public async Task Consume(ConsumeContext<ClubCreatedEvent> context)
            {
                if (_roomContext.Clubs.Find(context.Message.ClubId) == null)
                {
                    if (context.Message.Locations == null)
                    {
                        context.Message.Locations = new List<string>();
                        context.Message.Locations.Add("Default");
                    }

                    if (context.Message.Locations.Count == 0)
                    {
                        context.Message.Locations.Add("Default");
                    }
                var club = new Club()
                    {
                        ClubId = context.Message.ClubId,
                        Rooms = context.Message.Locations.Select(location => new Room()
                        {
                            ClubId = context.Message.ClubId,
                            Name = location
                        }).ToList()
                    };
                    _roomContext.Clubs.Add(club);
                    //Publish events.. 
                    var events = club.Rooms.Select(room => new RoomCreatedEvent()
                    {
                        RoomId = room.RoomId,
                        ClubId = room.ClubId,
                        Name = room.Name
                    }).ToList();

                    await _eventService.SaveEventAndDbContextChangesAsync(events);
                    foreach (var roomCreatedEvent in events)
                    { 
                        await _eventService.PublishEventAsync(roomCreatedEvent);
                    }
                    
                }
        }
    }
}
