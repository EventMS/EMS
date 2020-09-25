using System;
using System.Threading.Tasks;
using AutoMapper;
using EMS.Events;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;
using Club.API.Context;
using Club.API.Controllers.Request;
using HotChocolate.AspNetCore.Authorization;
using Identity.API;
using Serilog;
using TemplateWebHost.Customization.EventService;
namespace Club.API.GraphQlQueries
{
    public class ClubMutations
    {
        private readonly ClubContext _context;
        private readonly IMapper _mapper;
        private readonly IEventService _eventService;

        public ClubMutations(ClubContext context, IEventService template1EventService, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); ;
            _eventService = template1EventService ?? throw new ArgumentNullException(nameof(template1EventService));
            _mapper = mapper;
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        [Authorize(Roles = new[] { "Admin" })]
        public async Task<Context.Model.Club> UpdateClubAsync(Guid clubId, UpdateClubRequest request, [CurrentUserGlobalState] CurrentUser currentUser)
        {
            var item = await _context.Clubs.SingleOrDefaultAsync(ci => ci.ClubId == clubId);

            if (item == null)
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetMessage("The provided id is unknown.")
                        .SetCode("ID_UNKNOWN")
                        .Build());
            }

            item = _mapper.Map<Context.Model.Club>(request);
            item.ClubId = clubId;
            item.AdminId = currentUser.UserId;
            _context.Clubs.Update(item);

            var @event = _mapper.Map<ClubUpdatedEvent>(item);
            await _eventService.SaveEventAndDbContextChangesAsync(@event);
            await _eventService.PublishThroughEventBusAsync(@event);

            return item;
        }

        [Authorize]
        public async Task<Context.Model.Club> CreateClubAsync(CreateClubRequest request, [CurrentUserGlobalState] CurrentUser currentUser)
        {
            var item = _mapper.Map<Context.Model.Club>(request);
            item.AdminId = currentUser.UserId;
            _context.Clubs.Add(item);

            var @event = _mapper.Map<ClubCreatedEvent>(item);
            Log.Information(item.AdminId.ToString());
            await _eventService.SaveEventAndDbContextChangesAsync(@event);
            await _eventService.PublishThroughEventBusAsync(@event);

            return item;
        }

        [Authorize(Roles = new[] { "Admin" })]
        public async Task<Context.Model.Club> DeleteClubAsync(Guid clubId)
        {
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
            await _eventService.PublishThroughEventBusAsync(@event);
            return item;
        }
    }
}
