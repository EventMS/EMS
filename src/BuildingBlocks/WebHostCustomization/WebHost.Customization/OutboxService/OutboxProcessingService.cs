using System;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EMS.BuildingBlocks.IntegrationEventLogEF.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using EMS.TemplateWebHost.Customization.EventService;

namespace EMS.TemplateWebHost.Customization.OutboxService
{
    /// <summary>
    /// This is the executing client that does the actual work of checking for failed tasks
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class OutboxProcessingService<T> : IOutboxProcessingService where T:DbContext
    {
        private readonly ILogger _logger;
        private readonly IEventService _eventService;
        private readonly IEventLogService _eventLogService;
        private readonly T _context;

        public OutboxProcessingService(ILogger<OutboxProcessingService<T>> logger, 
            IEventService eventService, 
            Func<DbConnection, IEventLogService> eventLogServiceFactory, T context)
        {
            _logger = logger;
            _eventService = eventService;
            _context = context;
            var factory = eventLogServiceFactory ?? throw new ArgumentNullException(nameof(eventLogServiceFactory));
            _eventLogService = factory(_context.Database.GetDbConnection());
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            var events = await _eventLogService.RetrieveEventLogsFailedToPublishAsync();
            if(events.Count() == 0)
            {
                return;
            }
            _logger.LogInformation("Outbox is attempting to publish failed events, current count is: {Count}", events.Count());

            foreach (var eventLogEntry in events)
            {
                //Required possible gracefull shutdown. 
                if (stoppingToken.IsCancellationRequested)
                {
                    return;
                }

                await _eventService.PublishEventAsync(eventLogEntry.Event, eventLogEntry.Type);
            }
            events = await _eventLogService.RetrieveEventLogsFailedToPublishAsync();
            _logger.LogInformation("New count of outbox failed events is: {Count}", events.Count());
        }
    }
}