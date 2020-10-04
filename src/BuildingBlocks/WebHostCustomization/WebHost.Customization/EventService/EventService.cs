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
using EMS.TemplateWebHost.Customization.Settings;

namespace EMS.TemplateWebHost.Customization.EventService
{
    public class EventService<TContext> : IEventService where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly IEventLogService _eventLogService;
        private readonly ILogger<EventService<TContext>> _logger;
        private readonly BaseSettings _settings;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ISendEndpoint _sendEndpoint;

        public EventService(
            ILogger<EventService<TContext>> logger,
            TContext catalogContext,
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

        public async Task PublishEventAsync<TEvent>(TEvent evt, Type type = null) where TEvent : Event
        {
            try
            {
                _logger.LogInformation("----- Publishing event: {EventId_published} from {AppName} - ({@Event})", evt.Id, _settings.SubscriptionClientName, evt);

                await _eventLogService.MarkEventAsInProgressAsync(evt.Id);
                await Publish(evt, type);
                await _eventLogService.MarkEventAsPublishedAsync(evt.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Publishing event: {EventId} from {AppName} - ({@Event})", evt.Id, _settings.SubscriptionClientName, evt);
                await _eventLogService.MarkEventAsFailedAsync(evt.Id);
            }
        }

        private async Task Publish<TEvent>(TEvent evt, Type type) where TEvent : Event
        {
            if (type == null)
            {
                await _publishEndpoint.Publish(evt);
            }
            else
            {
                await _publishEndpoint.Publish(evt, type);
            }
        }

        public async Task SaveContextThenPublishEvent(Event evt, Func<Task> action = null)
        {
            await SaveEventAndDbContextChangesAsync(evt, action);
            await PublishEventAsync(evt);
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
