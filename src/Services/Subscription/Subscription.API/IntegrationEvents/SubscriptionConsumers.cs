using System.Threading.Tasks;
using EMS.Events;
using MassTransit;
using Serilog;
using Subscription.API.Context;
using Club = Subscription.API.Context.Club;

namespace Subscription.API.IntegrationEvents
{
    public class ClubCreatedEventConsumer :
            IConsumer<ClubCreatedEvent>
    {
        private readonly SubscriptionContext _subscriptionContext;

        public ClubCreatedEventConsumer(SubscriptionContext subscriptionContext)
        {
            _subscriptionContext = subscriptionContext;
        }

        public async Task Consume(ConsumeContext<ClubCreatedEvent> context)
        {
           var club =  _subscriptionContext.Clubs.Find(context.Message.ClubId);
           if (club == null)
           {
               _subscriptionContext.Clubs.Add(new Context.Club()
               {
                   ClubId = context.Message.ClubId
               });
               await _subscriptionContext.SaveChangesAsync();
           }
        }
        }
    }
