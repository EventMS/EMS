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
using EMS.TemplateWebHost.Customization.StartUp;
using HotChocolate.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace EMS.ClubMember_Services.API.GraphQlQueries
{
    public class ClubMemberMutations : BaseMutations
    {
        private readonly ClubMemberContext _context;
        private readonly IMapper _mapper;
        private readonly IEventService _eventService;

        public ClubMemberMutations(ClubMemberContext context, IEventService template1EventService, IMapper mapper, IAuthorizationService authorizationService) : base(authorizationService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); ;
            _eventService = template1EventService ?? throw new ArgumentNullException(nameof(template1EventService));
            _mapper = mapper;
        }

        [HotChocolate.AspNetCore.Authorization.Authorize]
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

            await IsAdminIn(item.ClubId);

            item.ClubSubscriptionId = request.ClubSubscriptionId;
            _context.ClubMembers.Update(item);

            var @event = _mapper.Map<ClubMemberUpdatedEvent>(item);
            await _eventService.SaveEventAndDbContextChangesAsync(@event);
            await _eventService.PublishEventAsync(@event);
            return item;
        }

        [HotChocolate.AspNetCore.Authorization.Authorize]
        public async Task<ClubMember> CreateClubMemberAsync(CreateClubMemberRequest request)
        {
            var subscription = await _context.ClubSubscriptions.FindAsync(request.ClubSubscriptionId);

            await IsAdminIn(subscription.ClubId);
            var item = _mapper.Map<ClubMember>(request);
            item.ClubId = subscription.ClubId;
            _context.ClubMembers.Add(item);

            var @event = _mapper.Map<ClubMemberCreatedEvent>(item);
            await _eventService.SaveEventAndDbContextChangesAsync(@event);
            await _eventService.PublishEventAsync(@event);

            return item;
        }

        [HotChocolate.AspNetCore.Authorization.Authorize]
        public async Task<ClubMember> DeleteClubMemberAsync(Guid userId, Guid clubId)
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
            await IsAdminIn(item.ClubId);

            _context.ClubMembers.Remove(item);

            var @event = _mapper.Map<ClubMemberDeletedEvent>(item);
            await _eventService.SaveEventAndDbContextChangesAsync(@event);
            await _eventService.PublishEventAsync(@event);
            return item;
        }
    }
}
