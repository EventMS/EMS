using System;
using System.Threading.Tasks;
using EMS.Events;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;
using Club.API.Context;
using Club.API.Controllers.Request;
using TemplateWebHost.Customization.IntegrationEventService;

namespace Club.API.GraphQlQueries
{
    public class ClubMutations
    {
        private readonly ClubContext _context;
        private readonly IIntegrationEventService _integrationEventService;

        public ClubMutations(ClubContext context, IIntegrationEventService template1IntegrationEventService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); ;
            _integrationEventService = template1IntegrationEventService ?? throw new ArgumentNullException(nameof(template1IntegrationEventService));
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task<Context.Model.Club> UpdateClubAsync(Guid id, UpdateClubRequest request)
        {
            var item = await _context.Clubs.SingleOrDefaultAsync(ci => ci.Id == id);

            if (item == null)
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetMessage("The provided id is unknown.")
                        .SetCode("ID_UNKNOWN")
                        .Build());
            }

            item.Name = request.Name;
            _context.Clubs.Update(item);

            var @event = new ClubUpdatedIntegrationEvent(item.Id, item.Name);
            await _integrationEventService.SaveEventAndDbContextChangesAsync(@event);
            await _integrationEventService.PublishThroughEventBusAsync(@event);

            return item;
        }


        public async Task<Context.Model.Club> CreateClubAsync(CreateClubRequest request)
        {
            var item = new Context.Model.Club()
            {
                Name = request.Name
            };

            _context.Clubs.Add(item);

            var @event = new ClubCreatedIntegrationEvent(item.Id, item.Name);
            await _integrationEventService.SaveEventAndDbContextChangesAsync(@event);
            await _integrationEventService.PublishThroughEventBusAsync(@event);

            return item;
        }

        public async Task<Context.Model.Club> DeleteClubAsync(Guid id)
        {
            var item = await _context.Clubs.SingleOrDefaultAsync(ci => ci.Id == id);

            if (item == null)
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetMessage("The provided id is unknown.")
                        .SetCode("ID_UNKNOWN")
                        .Build());
            }

            _context.Clubs.Remove(item);

            var @event = new ClubDeletedIntegrationEvent(item.Id);
            await _integrationEventService.SaveEventAndDbContextChangesAsync(@event);
            await _integrationEventService.PublishThroughEventBusAsync(@event);
            return item;
        }
    }
}
