using System;
using System.Threading.Tasks;
using EMS.Events;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;
using Permission.API.Context;
using Permission.API.Controllers.Request;
using TemplateWebHost.Customization.IntegrationEventService;

namespace Permission.API.GraphQlQueries
{
    public class PermissionMutations
    {
        private readonly PermissionContext _context;
        private readonly IIntegrationEventService _integrationEventService;

        public PermissionMutations(PermissionContext context, IIntegrationEventService template1IntegrationEventService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); ;
            _integrationEventService = template1IntegrationEventService ?? throw new ArgumentNullException(nameof(template1IntegrationEventService));
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task<Context.Model.Permission> UpdatePermissionAsync(Guid id, UpdatePermissionRequest request)
        {
            var item = await _context.Permissions.SingleOrDefaultAsync(ci => ci.Id == id);

            if (item == null)
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetMessage("The provided id is unknown.")
                        .SetCode("ID_UNKNOWN")
                        .Build());
            }

            item.Name = request.Name;
            _context.Permissions.Update(item);

            var @event = new PermissionUpdatedIntegrationEvent(item.Id, item.Name);
            await _integrationEventService.SaveEventAndDbContextChangesAsync(@event);
            await _integrationEventService.PublishThroughEventBusAsync(@event);

            return item;
        }


        public async Task<Context.Model.Permission> CreatePermissionAsync(CreatePermissionRequest request)
        {
            var item = new Context.Model.Permission()
            {
                Name = request.Name
            };

            _context.Permissions.Add(item);

            var @event = new PermissionCreatedIntegrationEvent(item.Id, item.Name);
            await _integrationEventService.SaveEventAndDbContextChangesAsync(@event);
            await _integrationEventService.PublishThroughEventBusAsync(@event);

            return item;
        }

        public async Task<Context.Model.Permission> DeletePermissionAsync(Guid id)
        {
            var item = await _context.Permissions.SingleOrDefaultAsync(ci => ci.Id == id);

            if (item == null)
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetMessage("The provided id is unknown.")
                        .SetCode("ID_UNKNOWN")
                        .Build());
            }

            _context.Permissions.Remove(item);

            var @event = new PermissionDeletedIntegrationEvent(item.Id);
            await _integrationEventService.SaveEventAndDbContextChangesAsync(@event);
            await _integrationEventService.PublishThroughEventBusAsync(@event);
            return item;
        }
    }
}
