using AutoMapper;
using EMS.ClubMember_Services.API.Context;
using EMS.ClubMember_Services.API.Context.Model;
using EMS.Events;
using EMS.TemplateWebHost.Customization.EventService;
using MassTransit;
using Serilog;
using System;
using System.Threading.Tasks;
using TemplateWebHost.Customization.BasicConsumers;

namespace EMS.ClubMember_Services.API.Events
{
    public class ClubSubscriptionCreatedEventConsumer :
           IConsumer<ClubSubscriptionCreatedEvent>
    {
        private readonly ClubMemberContext _context;
        private readonly IMapper _mapper;
        private readonly IEventService _eventService;

        public ClubSubscriptionCreatedEventConsumer(ClubMemberContext context, IMapper mapper, IEventService eventService)
        {
            _context = context;
            _mapper = mapper;
            _eventService = eventService;
        }
        public async Task Consume(ConsumeContext<ClubSubscriptionCreatedEvent> context)
        {
            var clubSub = _context.ClubSubscriptions.Find(context.Message.ClubSubscriptionId);
            if(clubSub == null)
            {
                clubSub = new ClubSubscription()
                {
                    ClubSubscriptionId = context.Message.ClubSubscriptionId,
                    ClubId = context.Message.ClubId
                };
                await _context.ClubSubscriptions.AddAsync(clubSub);
                if(context.Message.Name.Equals("Club admin membership"))
                {
                    var member = new ClubMember()
                    {
                        ClubId = context.Message.ClubId,
                        ClubSubscriptionId = context.Message.ClubSubscriptionId,
                        UserId = context.Headers.Get<Guid>("id").Value
                    };
                    _context.ClubMembers.Add(member);
                    var e = _mapper.Map<ClubMemberCreatedEvent>(member);
                    await _eventService.SaveEventAndDbContextChangesAsync(e);
                    await _eventService.PublishEventAsync(e);
                }
                else
                {
                   await _context.SaveChangesAsync();
                }
            }
        }
    }
}


