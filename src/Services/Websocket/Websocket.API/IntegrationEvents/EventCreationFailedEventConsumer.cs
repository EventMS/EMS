using System.Threading.Tasks;
using EMS.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace EMS.Websocket_Services.API.Events
{
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
