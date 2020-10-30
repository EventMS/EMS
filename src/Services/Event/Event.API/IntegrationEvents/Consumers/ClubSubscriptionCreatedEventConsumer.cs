

using System.Linq;
using System.Threading.Tasks;
using EMS.Event_Services.API.Context;
using EMS.Event_Services.API.Context.Model;
using EMS.Events;
using MassTransit;
using MassTransit.Courier.Contracts;
using Serilog;

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
            var subscription = _context.Subscriptions.Find(context.Message.ClubSubscriptionId);
            if (subscription == null)
            {
                if(context.Message.ReferenceId != null)
                {
                    foreach (var messageEventPrice in _context.EventPrices
                        .Where(evtPrice => evtPrice.ClubSubscriptionId == context.Message.ReferenceId).ToList())
                    {
                        _context.EventPrices.Add(new Context.Model.EventPrice()
                        {
                            Price = messageEventPrice.Price,
                            EventId = messageEventPrice.EventId,
                            ClubSubscriptionId = context.Message.ClubSubscriptionId
                        });
                    }
                }

                _context.Subscriptions.Add(new ClubSubscription()
                {
                    ClubSubscriptionId = context.Message.ClubSubscriptionId,
                    ClubId = context.Message.ClubId
                });

                await _context.SaveChangesAsync();
            }
        }
    }
}