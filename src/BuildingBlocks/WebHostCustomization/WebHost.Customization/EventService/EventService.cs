using System;
using System.Data.Common;
using System.Threading.Tasks;
using EMS.BuildingBlocks.EventLogEF;
using EMS.BuildingBlocks.EventLogEF.Utilities;
using EMS.BuildingBlocks.IntegrationEventLogEF.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TemplateWebHost.Customization.Settings;

namespace TemplateWebHost.Customization.EventService
{
    public class EventService<T> : IEventService where T : DbContext
    {
        private readonly T _context;
        private readonly IEventLogService _eventLogService;
        private readonly ILogger<EventService<T>> _logger;
        private readonly BaseSettings _settings;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ISendEndpoint _sendEndpoint;

        public EventService(
            ILogger<EventService<T>> logger,
            T catalogContext,
            Func<DbConnection, IEventLogService> eventLogServiceFactory,
            IOptions<BaseSettings> settings, 
            IPublishEndpoint publishEndpoint)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _context = catalogContext ?? throw new ArgumentNullException(nameof(catalogContext));
            _publishEndpoint = publishEndpoint;
            var factory = eventLogServiceFactory ?? throw new ArgumentNullException(nameof(eventLogServiceFactory));
            _eventLogService = factory(_context.Database.GetDbConnection());
            _settings = settings.Value;
        }

        public async Task PublishThroughEventBusAsync<T2>(T2 evt) where T2 : Event
        {
            try
            {
                _logger.LogInformation("----- Publishing integration event: {EventId_published} from {AppName} - ({@Event})", evt.Id, _settings.SubscriptionClientName, evt);

                await _eventLogService.MarkEventAsInProgressAsync(evt.Id);
                await _publishEndpoint.Publish(evt);
                await _eventLogService.MarkEventAsPublishedAsync(evt.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Publishing integration event: {EventId} from {AppName} - ({@Event})", evt.Id, _settings.SubscriptionClientName, evt);
                await _eventLogService.MarkEventAsFailedAsync(evt.Id);
            }
        }

        public async Task SaveContextThenPublishEvent(Event evt, Func<Task> action = null)
        {
            await SaveEventAndDbContextChangesAsync(evt, action);
            await PublishThroughEventBusAsync(evt);
        }

        public async Task SaveEventAndDbContextChangesAsync(Event evt, Func<Task> action = null)
        {
            _logger.LogInformation("----- EventService - Saving changes and event: {EventId}", evt.Id);

            //Use of an EF Core resiliency strategy when using multiple DbContexts within an explicit BeginTransaction():
            //See: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency            
            await ResilientTransaction.New(_context).ExecuteAsync(async () =>
            {
                if (action != null)
                {
                    await action();
                }
                // Achieving atomicity between original database operation and the EventLog thanks to a local transaction
                await _context.SaveChangesAsync();
                await _eventLogService.SaveEventAsync(evt, _context.Database.CurrentTransaction);
            });
        }
    }
}
