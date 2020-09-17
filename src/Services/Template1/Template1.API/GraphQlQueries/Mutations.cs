using System;
using System.Threading.Tasks;
using EMS.Events;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;
using Template1.API.Context;
using Template1.API.Controllers.Request;
using TemplateWebHost.Customization.IntegrationEvents;

namespace Template1.API.GraphQlQueries
{
    public class Mutations
    {
        private readonly Template1Context _context;
        private readonly IIntegrationEventService _integrationEventService;

        public Mutations(Template1Context context, IIntegrationEventService template1IntegrationEventService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); ;
            _integrationEventService = template1IntegrationEventService ?? throw new ArgumentNullException(nameof(template1IntegrationEventService));
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task<Context.Model.Template1> UpdateTemplate1Async(Guid id, UpdateTemplate1Request request)
        {
            var item = await _context.Template1s.SingleOrDefaultAsync(ci => ci.Id == id);

            if (item == null)
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetMessage("The provided id is unknown.")
                        .SetCode("ID_UNKNOWN")
                        .Build());
            }

            item.Name = request.Name;
            _context.Template1s.Update(item);

            var @event = new Template1UpdatedIntegrationEvent(item.Id, item.Name);
            await _integrationEventService.SaveEventAndDbContextChangesAsync(@event);
            await _integrationEventService.PublishThroughEventBusAsync(@event);

            return item;
        }


        public async Task<Context.Model.Template1> CreateTemplate1Async(CreateTemplate1Request request)
        {
            var item = new Context.Model.Template1()
            {
                Name = request.Name
            };

            _context.Template1s.Add(item);

            var @event = new Template1CreatedIntegrationEvent(item.Id, item.Name);
            await _integrationEventService.SaveEventAndDbContextChangesAsync(@event);
            await _integrationEventService.PublishThroughEventBusAsync(@event);

            return item;
        }

        public async Task<Context.Model.Template1> DeleteTemplate1Async(Guid id)
        {
            var item = await _context.Template1s.SingleOrDefaultAsync(ci => ci.Id == id);

            if (item == null)
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetMessage("The provided id is unknown.")
                        .SetCode("ID_UNKNOWN")
                        .Build());
            }

            _context.Template1s.Remove(item);

            var @event = new Template1DeletedIntegrationEvent(item.Id);
            await _integrationEventService.SaveEventAndDbContextChangesAsync(@event);
            await _integrationEventService.PublishThroughEventBusAsync(@event);
            return item;
        }
    }
}