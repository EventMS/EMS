using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Club.API.Context;
using EMS.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TemplateWebHost.Customization.EventService;

namespace Club.API.Events
{
    public class UserIsClubMemberClubConsumer :
            IConsumer<UserIsClubMember>
        {
        private readonly ClubContext _context;
        private readonly IEventService _eventService;

        public UserIsClubMemberClubConsumer(ClubContext context, IEventService eventService)
        {
            _context = context;
            _eventService = eventService;
        }

        public async Task Consume(ConsumeContext<UserIsClubMember> context)
            {
            var club = await _context.Clubs.SingleOrDefaultAsync(ci => ci.ClubId == context.Message.ClubId);
            if(club == null)
            {
                return;
            }
            if (club.InstructorIds == null)
            {
                club.InstructorIds = new HashSet<Guid>();
            }
            club.InstructorIds.Add(context.Message.UserId);
            _context.Clubs.Update(club);
            var @event = new InstructorAdded()
            {
                ClubId = context.Message.ClubId,
                UserId = context.Message.UserId
            };
            await _eventService.SaveEventAndDbContextChangesAsync(@event);
            await _eventService.PublishEventAsync(@event);
        }
        }
    }
