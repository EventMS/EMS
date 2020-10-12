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
            if (_subscriptionContext.ClubSubscriptions.Find(context.Message.SubscriptionId) == null)
            {
                _subscriptionContext.ClubSubscriptions.Add(new ClubSubscription()
                {
                    ClubId = context.Message.ClubId,
                    ClubSubscriptionId = context.Message.SubscriptionId
                });
                await _subscriptionContext.SaveChangesAsync();
            }
        }
        }
}


