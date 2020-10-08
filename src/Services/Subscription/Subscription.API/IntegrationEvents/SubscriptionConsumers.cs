using System.Threading.Tasks;
using EMS.Events;
using MassTransit;
using EMS.Subscription_Services.API.Context;
namespace EMS.Subscription_Services.API.IntegrationEvents
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
               _subscriptionContext.Clubs.Add(new Club()
               {
                   ClubId = context.Message.ClubId
               });
               await _subscriptionContext.SaveChangesAsync();
           }
        }
    }
}
