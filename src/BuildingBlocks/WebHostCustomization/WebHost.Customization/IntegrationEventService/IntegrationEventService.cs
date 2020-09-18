using System;
using System.Data.Common;
using System.Threading.Tasks;
using EMS.BuildingBlocks.IntegrationEventLogEF;
using EMS.BuildingBlocks.IntegrationEventLogEF.Services;
using EMS.BuildingBlocks.IntegrationEventLogEF.Utilities;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TemplateWebHost.Customization.Settings;

namespace TemplateWebHost.Customization.IntegrationEventService
{
    public class IntegrationEventService<T> : IIntegrationEventService where T : DbContext
    {
        private readonly T _context;
        private readonly IIntegrationEventLogService _eventLogService;
        private readonly ILogger<IntegrationEventService<T>> _logger;
        private readonly BaseSettings _settings;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ISendEndpoint _sendEndpoint;

        public IntegrationEventService(
            ILogger<IntegrationEventService<T>> logger,
            T catalogContext,
            Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory,
            IOptions<BaseSettings> settings, 
            IPublishEndpoint publishEndpoint)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _context = catalogContext ?? throw new ArgumentNullException(nameof(catalogContext));
            _publishEndpoint = publishEndpoint;
            var factory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
            _eventLogService = factory(_context.Database.GetDbConnection());
            _settings = settings.Value;
        }

        public async Task PublishThroughEventBusAsync<T2>(T2 evt) where T2 : IntegrationEvent
        {
            try
            {
                _logger.LogInformation("----- Publishing integration event: {IntegrationEventId_published} from {AppName} - ({@IntegrationEvent})", evt.Id, _settings.SubscriptionClientName, evt);

                await _eventLogService.MarkEventAsInProgressAsync(evt.Id);
                await _publishEndpoint.Publish(evt);
                await _eventLogService.MarkEventAsPublishedAsync(evt.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", evt.Id, _settings.SubscriptionClientName, evt);
                await _eventLogService.MarkEventAsFailedAsync(evt.Id);
            }
        }

        public async Task SaveContextThenPublishEvent(IntegrationEvent evt, Func<Task> action = null)
        {
            await SaveEventAndDbContextChangesAsync(evt, action);
            await PublishThroughEventBusAsync(evt);
        }

        public async Task SaveEventAndDbContextChangesAsync(IntegrationEvent evt, Func<Task> action = null)
        {
            _logger.LogInformation("----- IntegrationEventService - Saving changes and integrationEvent: {IntegrationEventId}", evt.Id);

            //Use of an EF Core resiliency strategy when using multiple DbContexts within an explicit BeginTransaction():
            //See: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency            
            await ResilientTransaction.New(_context).ExecuteAsync(async () =>
            {
                if (action != null)
                {
                    await action();
                }
                // Achieving atomicity between original database operation and the IntegrationEventLog thanks to a local transaction
                await _context.SaveChangesAsync();
                await _eventLogService.SaveEventAsync(evt, _context.Database.CurrentTransaction);
            });
        }
    }
}
