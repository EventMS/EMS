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
using TemplateWebHost.Customization.IntegrationEventService;
namespace Club.API.GraphQlQueries
{
    public class ClubMutations
    {
        private readonly ClubContext _context;
        private readonly IMapper _mapper;
        private readonly IIntegrationEventService _integrationEventService;

        public ClubMutations(ClubContext context, IIntegrationEventService template1IntegrationEventService, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); ;
            _integrationEventService = template1IntegrationEventService ?? throw new ArgumentNullException(nameof(template1IntegrationEventService));
            _mapper = mapper;
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        [Authorize(Roles = new[] { "Admin" })]
        public async Task<Context.Model.Club> UpdateClubAsync(Guid clubId, UpdateClubRequest request)
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
            _context.Clubs.Update(item);

            var @event = _mapper.Map<ClubUpdatedIntegrationEvent>(item);
            await _integrationEventService.SaveEventAndDbContextChangesAsync(@event);
            await _integrationEventService.PublishThroughEventBusAsync(@event);

            return item;
        }

        [Authorize]
        public async Task<Context.Model.Club> CreateClubAsync(CreateClubRequest request, [CurrentUserGlobalState] CurrentUser currentUser)
        {
            var item = _mapper.Map<Context.Model.Club>(request);
            item.AdminId = currentUser.UserId;
            _context.Clubs.Add(item);

            var @event = _mapper.Map<ClubCreatedIntegrationEvent>(item);
            await _integrationEventService.SaveEventAndDbContextChangesAsync(@event);
            await _integrationEventService.PublishThroughEventBusAsync(@event);

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

            var @event = _mapper.Map<ClubDeletedIntegrationEvent>(item);
            await _integrationEventService.SaveEventAndDbContextChangesAsync(@event);
            await _integrationEventService.PublishThroughEventBusAsync(@event);
            return item;
        }
    }
}
