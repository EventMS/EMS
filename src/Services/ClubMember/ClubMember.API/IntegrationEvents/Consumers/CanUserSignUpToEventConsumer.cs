using System.Threading.Tasks;
using EMS.ClubMember_Services.API.Context;
using EMS.Events;
using EMS.TemplateWebHost.Customization.EventService;
using MassTransit;

namespace EMS.ClubMember_Services.API.Events
{
    public class CanUserSignUpToEventConsumer :
        IConsumer<CanUserSignUpToEvent>
    {
        private readonly ClubMemberContext _subscriptionContext;
        private readonly IEventService _eventService;
        public CanUserSignUpToEventConsumer(ClubMemberContext subscriptionContext, IEventService eventService)
        {
            _subscriptionContext = subscriptionContext;
            _eventService = eventService;
        }


        public async Task Consume(ConsumeContext<CanUserSignUpToEvent> context)
        {
            var member = _subscriptionContext.ClubMembers.Find(context.Message.ClubId, context.Message.UserId);
            if (member != null)
            {
                var @event = new IsEventFreeForSubscriptionEvent()
                {
                    ClubSubscriptionId = member.ClubSubscriptionId,
                    UserId = context.Message.UserId,
                    EventId = context.Message.EventId
                };
                await _eventService.SaveEventAndDbContextChangesAsync(@event);
                await _eventService.PublishEventAsync(@event);
            }
        }
    }
}