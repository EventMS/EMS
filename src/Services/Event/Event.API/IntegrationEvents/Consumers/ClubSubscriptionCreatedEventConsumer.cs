

using System.Linq;
using System.Threading.Tasks;
using EMS.Event_Services.API.Context;
using EMS.Event_Services.API.Context.Model;
using EMS.Events;
using MassTransit;
using MassTransit.Courier.Contracts;


namespace EMS.Event_Services.API.Events
{
    public class ClubSubscriptionCreatedEventConsumer :
        IConsumer<ClubSubscriptionCreatedEvent>
    {
        private readonly EventContext _context;

        public ClubSubscriptionCreatedEventConsumer(EventContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<ClubSubscriptionCreatedEvent> context)
        {
            var subscription = _context.Subscriptions.Find(context.Message.SubscriptionId);
            if (subscription == null)
            {
                _context.Subscriptions.Add(new ClubSubscription()
                {
                    ClubSubscriptionId = context.Message.SubscriptionId,
                    ClubId = context.Message.ClubId
                });
                
                //To be tested
                foreach (var messageEventPrice in context.Message.EventPrices)
                {
                    _context.ClubSubscriptionEventPrice.Add(new ClubSubscriptionEventPrice()
                    {
                        Price = messageEventPrice.Price,
                        EventId = messageEventPrice.EventId,
                        SubscriptionId = context.Message.SubscriptionId
                    });
                }


                await _context.SaveChangesAsync();
            }
        }
    }
}