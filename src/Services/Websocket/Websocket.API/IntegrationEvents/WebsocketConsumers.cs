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
                Log.Information("Received eventCreated event");

                var clubId = context.Message.ClubId;    

                await _hub.Clients.All.SendAsync(clubId.ToString("N")+"-EventCreated", new
                {
                    EventId = context.Message.EventId,
                    ClubId = context.Message.ClubId,
                    Name = context.Message.Name
                });
            }
        }

    public class EventCreationFailedEventConsumer :
            IConsumer<EventCreationFailedEvent>
    {
        private IHubContext<EventHub> _hub;

        public EventCreationFailedEventConsumer(IHubContext<EventHub> hub)
        {
            _hub = hub;
        }

        public async Task Consume(ConsumeContext<EventCreationFailedEvent> context)
        {
            var clubId = context.Message.ClubId;
            Log.Information("Received eventCreationFailed event");
            await _hub.Clients.All.SendAsync(clubId.ToString("N")+ "-EventCreationFailed", new
            {
                eventId = context.Message.EventId,
                reason = context.Message.Reason,
            });
        }
    }
}
