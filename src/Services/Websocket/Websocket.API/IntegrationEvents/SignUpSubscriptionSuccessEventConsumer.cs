using System.Threading.Tasks;
using EMS.Events;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace EMS.Websocket_Services.API.Events
{
    public class SignUpSubscriptionSuccessEventConsumer :
        IConsumer<SignUpSubscriptionSuccessEvent>
    {
        private IHubContext<EventHub> _hub;

        public SignUpSubscriptionSuccessEventConsumer(IHubContext<EventHub> hub)
        {
            _hub = hub;
        }

        public async Task Consume(ConsumeContext<SignUpSubscriptionSuccessEvent> context)
        {
            Log.Information("Received SignUpSubscriptionSuccessEvent event");
            var userId = context.Message.UserId;

            await _hub.Clients.User(userId.ToString()).SendAsync(userId + "-ClubSignup", new
            {
                UserId = context.Message.UserId,
                ClubSubscriptionId = context.Message.ClubSubscriptionId,
            });
        }
    }
}
