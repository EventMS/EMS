using System;
using System.Threading.Tasks;
using AutoMapper;
using EMS.Events;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;
using EMS.ClubMember_Services.API.Context;
using EMS.ClubMember_Services.API.Context.Model;
using EMS.ClubMember_Services.API.Controllers.Request;
using EMS.TemplateWebHost.Customization.EventService;

namespace EMS.ClubMember_Services.API.GraphQlQueries
{
    public class ClubMemberMutations
    {
        private readonly ClubMemberContext _context;
        private readonly IMapper _mapper;
        private readonly IEventService _eventService;

        public ClubMemberMutations(ClubMemberContext context, IEventService template1EventService, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); ;
            _eventService = template1EventService ?? throw new ArgumentNullException(nameof(template1EventService));
            _mapper = mapper;
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task<ClubMember> UpdateClubMemberAsync(UpdateClubMemberRequest request)
        {
            var item = await _context.ClubMembers.SingleOrDefaultAsync(ci => ci.UserId == request.UserId && ci.ClubId == request.ClubId);

            if (item == null)
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetMessage("The provided id is unknown.")
                        .SetCode("ID_UNKNOWN")
                        .Build());
            }

            item.NameOfSubscription = request.NameOfSubscription;
            _context.ClubMembers.Update(item);

            var @event = _mapper.Map<ClubMemberUpdatedEvent>(item);
            await _eventService.SaveEventAndDbContextChangesAsync(@event);
            await _eventService.PublishEventAsync(@event);
            return item;
        }

        
        public async Task<ClubMember> CreateClubMemberAsync(CreateClubMemberRequest request)
        {
            var item = _mapper.Map<ClubMember>(request);

            _context.ClubMembers.Add(item);

            var @event = _mapper.Map<ClubMemberCreatedEvent>(item);
            await _eventService.SaveEventAndDbContextChangesAsync(@event);
            await _eventService.PublishEventAsync(@event);

            return item;
        }

        public async Task<Context.Model.ClubMember> DeleteClubMemberAsync(Guid userId, Guid clubId)
        {
            var item = await _context.ClubMembers.SingleOrDefaultAsync(ci => ci.UserId == userId && ci.ClubId == clubId);

            if (item == null)
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetMessage("The provided id is unknown.")
                        .SetCode("ID_UNKNOWN")
                        .Build());
            }

            _context.ClubMembers.Remove(item);

            var @event = _mapper.Map<ClubMemberDeletedEvent>(item);
            await _eventService.SaveEventAndDbContextChangesAsync(@event);
            await _eventService.PublishEventAsync(@event);
            return item;
        }
    }
}
