

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
            var subscription = _context.Subscriptions.Find(context.Message.SubscriptionId);
            if (subscription == null)
            {
                ClubSubscription sub;
                if(context.Message.ReferenceId == null)
                {
                    var temp = _context.Subscriptions.FirstOrDefault(); //There are just one if no other is specified
                    if(temp == null)
                    {
                        Log.Information("Could not find a reference ID unexpectedly");                    
                        return;
                    }
                    context.Message.ReferenceId = temp.ClubSubscriptionId;
                };


                _context.Subscriptions.Add(new ClubSubscription()
                {
                    ClubSubscriptionId = context.Message.SubscriptionId,
                    ClubId = context.Message.ClubId
                });
                
                //To be tested
                foreach (var messageEventPrice in _context.ClubSubscriptionEventPrice
                    .Where(evtPrice => evtPrice.SubscriptionId == context.Message.ReferenceId).ToList())
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