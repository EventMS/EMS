using System;
using System.Threading.Tasks;
using AutoMapper;
using EMS.Club_Service.API.Context;
using EMS.Club_Service.API.Context.Model;
using EMS.Club_Service.API.Controllers.Request;
using EMS.Club_Service_Services.API;
using EMS.Events;
using EMS.TemplateWebHost.Customization.EventService;
using EMS.TemplateWebHost.Customization.StartUp;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace EMS.Club_Service.API.GraphQlQueries
{
    public class ClubMutations : BaseMutations
    {
        private readonly ClubContext _context;
        private readonly IMapper _mapper;
        private readonly IEventService _eventService;

        public ClubMutations(ClubContext context, IEventService template1EventService, IMapper mapper, IAuthorizationService authorizationService) : base(authorizationService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); ;
            _eventService = template1EventService ?? throw new ArgumentNullException(nameof(template1EventService));
            _mapper = mapper;
        }
        [HotChocolate.AspNetCore.Authorization.Authorize]
        public async Task<Club> UpdateClubAsync(Guid clubId, UpdateClubRequest request)
        {
            await IsAdminIn(clubId);
            var item = await _context.Clubs.SingleOrDefaultAsync(ci => ci.ClubId == clubId);

            if (item == null)
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetMessage("The provided id is unknown.")
                        .SetCode("ID_UNKNOWN")
                        .Build());
            }
            _mapper.Map(request, item);
            _context.Clubs.Update(item);

            var @event = _mapper.Map<ClubUpdatedEvent>(item);
            await _eventService.SaveEventAndDbContextChangesAsync(@event);
            await _eventService.PublishEventAsync(@event);

            return item;
        }

        [HotChocolate.AspNetCore.Authorization.Authorize]
        public async Task<Club> CreateClubAsync(CreateClubRequest request, [CurrentUserGlobalState] CurrentUser currentUser)
        {
            var item = _mapper.Map<Club>(request);
            item.AdminId = currentUser.UserId;
            _context.Clubs.Add(item);

            var @event = _mapper.Map<ClubCreatedEvent>(item);
            @event.Locations = request.Locations;
            Log.Information(item.AdminId.ToString());
            await _eventService.SaveEventAndDbContextChangesAsync(@event);
            await _eventService.PublishEventAsync(@event);

            return item;
        }
        [HotChocolate.AspNetCore.Authorization.Authorize]
        public async Task<Club> DeleteClubAsync(Guid clubId)
        {
            await IsAdminIn(clubId);
            var item = await _context.Clubs.SingleOrDefaultAsync(ci => ci.ClubId == clubId);

            if (item == null)
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetMessage("The provided id is unknown.")
                        .SetCode("ID_UNKNOWN")
                        .Build());
            }

            _context.Clubs.Remove(item);

            var @event = _mapper.Map<ClubDeletedEvent>(item);
            await _eventService.SaveEventAndDbContextChangesAsync(@event);
            await _eventService.PublishEventAsync(@event);
            return item;
        }
        [HotChocolate.AspNetCore.Authorization.Authorize]
        public async Task<Club> AddInstructorAsync(Guid clubId, Guid instructorId)
        {
            await IsAdminIn(clubId);
            var club = await _context.Clubs.SingleOrDefaultAsync(ci => ci.ClubId == clubId);
            if (club == null)
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetMessage("The provided id is unknown.")
                        .SetCode("ID_UNKNOWN")
                        .Build());
            }

            var @event = new IsUserClubMemberEvent()
            {
                ClubId = clubId,
                UserId = instructorId
            };
            await _eventService.SaveEventAndDbContextChangesAsync(@event);
            await _eventService.PublishEventAsync(@event);
            return club;
        }
        [HotChocolate.AspNetCore.Authorization.Authorize]
        public async Task<Club> RemoveInstructorAsync(Guid clubId, Guid instructorId)
        {
            await IsAdminIn(clubId);
            var club = await _context.Clubs.SingleOrDefaultAsync(ci => ci.ClubId == clubId);
            if (club == null)
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetMessage("The provided id is unknown.")
                        .SetCode("ID_UNKNOWN")
                        .Build());
            }

            if (club.InstructorIds.Remove(instructorId))
            {
                var @event = new InstructorDeletedEvent()
                {
                    ClubId = clubId,
                    UserId = instructorId
                };
                await _eventService.SaveEventAndDbContextChangesAsync(@event);
                await _eventService.PublishEventAsync(@event);
            }
            return club;
        }

    }
}
