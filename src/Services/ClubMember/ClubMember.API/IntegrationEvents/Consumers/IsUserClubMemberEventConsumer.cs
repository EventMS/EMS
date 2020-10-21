using System.Threading.Tasks;
using EMS.ClubMember_Services.API.Context;
using EMS.Events;
using EMS.TemplateWebHost.Customization.EventService;
using MassTransit;

namespace EMS.ClubMember_Services.API.Events
{
    public class IsUserClubMemberEventConsumer :
        IConsumer<IsUserClubMemberEvent>
    {
        private readonly ClubMemberContext _subscriptionContext;
        private readonly IEventService _eventService;
        public IsUserClubMemberEventConsumer(ClubMemberContext subscriptionContext, IEventService eventService)
        {
            _subscriptionContext = subscriptionContext;
            _eventService = eventService;
        }


        public async Task Consume(ConsumeContext<IsUserClubMemberEvent> context)
        {
            if (_subscriptionContext.ClubMembers.Find(context.Message.UserId, context.Message.ClubId) != null)
            {
                var @event = new UserIsClubMemberEvent()
                {
                    ClubId = context.Message.ClubId,
                    UserId = context.Message.UserId
                };
                await _eventService.SaveEventAndDbContextChangesAsync(@event);
                await _eventService.PublishEventAsync(@event);
            }
        }
    }
}