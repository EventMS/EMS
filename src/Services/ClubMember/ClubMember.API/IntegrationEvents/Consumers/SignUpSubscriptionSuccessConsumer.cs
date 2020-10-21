using System.Threading.Tasks;
using AutoMapper;
using EMS.ClubMember_Services.API.Context;
using EMS.ClubMember_Services.API.Context.Model;
using EMS.Events;
using EMS.TemplateWebHost.Customization.EventService;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace EMS.ClubMember_Services.API.Events
{
    public class SignUpSubscriptionSuccessConsumer :
        IConsumer<SignUpSubscriptionSuccess>
    {
        private readonly ClubMemberContext _context;
        private readonly IMapper _mapper;
        private readonly IEventService _eventService;
        public SignUpSubscriptionSuccessConsumer(ClubMemberContext context, IMapper mapper, IEventService eventService)
        {
            _context = context;
            _mapper = mapper;
            _eventService = eventService;
        }

        public async Task Consume(ConsumeContext<SignUpSubscriptionSuccess> context)
        {
            var sub = await _context.ClubSubscriptions.FindAsync(context.Message.ClubSubscriptionId);
            if (sub == null)
            {
                return;
            }
            var member = await _context.ClubMembers.FirstOrDefaultAsync(ep =>
                ep.ClubId == sub.ClubId && ep.UserId == context.Message.UserId);

            if (member == null)
            {
                member = new ClubMember()
                {
                    ClubSubscriptionId = context.Message.ClubSubscriptionId,
                    UserId = context.Message.UserId,
                    ClubId = sub.ClubId
                };
                await _context.ClubMembers.AddAsync(member);
                var e = _mapper.Map<ClubMemberCreatedEvent>(member);
                await _eventService.SaveEventAndDbContextChangesAsync(e);
                await _eventService.PublishEventAsync(e);
            }
            else
            {
                member.ClubSubscriptionId = context.Message.ClubSubscriptionId;
                _context.ClubMembers.Update(member);
                var e = _mapper.Map<ClubMemberUpdatedEvent>(member);
                await _eventService.SaveEventAndDbContextChangesAsync(e);
                await _eventService.PublishEventAsync(e);
            }

        }
    }
}