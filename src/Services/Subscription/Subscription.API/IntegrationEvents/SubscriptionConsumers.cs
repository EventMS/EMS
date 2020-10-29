using System.Threading.Tasks;
using AutoMapper;
using EMS.Events;
using MassTransit;
using EMS.Subscription_Services.API.Context;
using TemplateWebHost.Customization.BasicConsumers;
using Stripe;
using EMS.TemplateWebHost.Customization.EventService;

namespace EMS.Subscription_Services.API.IntegrationEvents
{
    public class ClubCreatedEventConsumer :
            IConsumer<ClubCreatedEvent>
    {
        private readonly SubscriptionContext _context;
        private readonly IMapper _mapper;
        private readonly IEventService _eventService;
        public ClubCreatedEventConsumer(SubscriptionContext context, IMapper mapper, IEventService eventService)
        {
            _context = context;
            _mapper = mapper;
            _eventService = eventService;
        }

        public async Task Consume(ConsumeContext<ClubCreatedEvent> context)
        {
            var club = await _context.Clubs.FindAsync(context.Message.ClubId);
            if(club == null)
            {
                await _context.Clubs.AddAsync(new Club()
                {
                    ClubId = context.Message.ClubId
                });
                var sub = new ClubSubscription()
                {
                    ClubId = context.Message.ClubId,
                    Name = "Club admin membership",
                    Price = 0
                };
                await _context.ClubSubscriptions.AddAsync(sub);
                var e = _mapper.Map<ClubSubscriptionCreatedEvent>(sub);
                await _eventService.SaveEventAndDbContextChangesAsync(e);
                await _eventService.PublishEventAsync(e);
            }
        }
    }
}
