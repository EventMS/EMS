using System.Threading.Tasks;
using EMS.ClubMember_Services.API.Context;
using EMS.ClubMember_Services.API.Context.Model;
using EMS.Events;
using MassTransit;
using Serilog;

namespace EMS.ClubMember_Services.API.Events
{
    public class ClubSubscriptionCreatedEventConsumer :
            IConsumer<ClubSubscriptionCreatedEvent>
        {
            private readonly ClubMemberContext _subscriptionContext;

            public ClubSubscriptionCreatedEventConsumer(ClubMemberContext subscriptionContext)
            {
                _subscriptionContext = subscriptionContext;
            }


        public async Task Consume(ConsumeContext<ClubSubscriptionCreatedEvent> context)
        {
            if (_subscriptionContext.ClubSubscriptions.Find(context.Message.ClubId, context.Message.Name) == null)
            {
                _subscriptionContext.ClubSubscriptions.Add(new ClubSubscription()
                {
                    ClubId = context.Message.ClubId,
                    NameOfSubscription = context.Message.Name
                });
                await _subscriptionContext.SaveChangesAsync();
            }
        }
        }
}


