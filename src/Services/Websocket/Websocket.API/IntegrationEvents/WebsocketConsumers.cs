using System.Threading.Tasks;
using EMS.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace EMS.Websocket_Services.API.Events
{
    public class EventCreatedEventConsumer :
            IConsumer<EventCreatedEvent>
        {
            private IHubContext<EventHub> _hub;

            public EventCreatedEventConsumer(IHubContext<EventHub> hub)
            {
                _hub = hub;
            }

            public async Task Consume(ConsumeContext<EventCreatedEvent> context)
            {
                Log.Information("Received event");
                await _hub.Clients.All.SendAsync("eventcreated", new
                {
                    EvemtId = context.Message.EventId,
                    ClubId = context.Message.ClubId,
                    Name = context.Message.Name
                });
            }
        }
    }
