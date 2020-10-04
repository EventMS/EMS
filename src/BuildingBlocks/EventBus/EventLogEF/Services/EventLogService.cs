using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using EMS.BuildingBlocks.IntegrationEventLogEF;
using EMS.BuildingBlocks.IntegrationEventLogEF.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EMS.BuildingBlocks.EventLogEF.Services
{
    public class EventLogService : IEventLogService
    {
        private readonly EventLogContext _eventLogContext;
        private readonly DbConnection _dbConnection;
        private readonly List<Type> _eventTypes;

        public EventLogService(DbContextOptions<EventLogContext> options)
        {
           _eventLogContext = new EventLogContext(options);

            _eventTypes = Assembly.Load(Assembly.GetEntryAssembly().FullName)
                .GetTypes()
                .Where(t => t.Name.EndsWith(nameof(Event)))
                .ToList();
        }

        public EventLogService(DbConnection dbConnection)
        {
            _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));
            _eventLogContext = new EventLogContext(
                new DbContextOptionsBuilder<EventLogContext>()
                    .UseSqlServer(_dbConnection)
                    .Options);

            _eventTypes = Assembly.Load(Assembly.GetEntryAssembly().FullName)
                .GetTypes()
                .Where(t => t.Name.EndsWith(nameof(Event)))
                .ToList();
        }

        public async Task<IEnumerable<EventLogEntry>> RetrieveEventLogsFailedToPublishAsync()
        {
            var currentDate = DateTime.UtcNow;
            var result = await _eventLogContext.EventLogs
                .Where(e =>e.State == EventStateEnum.PublishedFailed
                || ((e.State == EventStateEnum.InProgress 
                     || e.State == EventStateEnum.NotPublished) && e.CreationTime.AddMinutes(5) < currentDate)
                ).ToListAsync();

            if(result.Any()){
                return result.OrderBy(o => o.CreationTime)
                    .Select(e => e.DeserializeJsonContent(GetTypeOfEvent(e)));
            }
            
            return new List<EventLogEntry>();
        }

        private Type GetTypeOfEvent(EventLogEntry eventLog)
        {
            return _eventTypes.Find(t => t.Name == eventLog.EventTypeShortName);
        }

        public async Task SaveEventAsync(Event @event, IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            var eventLogEntry = new EventLogEntry(@event, transaction.TransactionId);

            _eventLogContext.Database.UseTransaction(transaction.GetDbTransaction());
            _eventLogContext.EventLogs.Add(eventLogEntry);
            await _eventLogContext.SaveChangesAsync();
            _eventLogContext.Database.UseTransaction(null);
        }

        public Task MarkEventAsPublishedAsync(Guid eventId)
        {
            return UpdateEventStatus(eventId, EventStateEnum.Published);
        }

        public Task MarkEventAsInProgressAsync(Guid eventId)
        {
            return UpdateEventStatus(eventId, EventStateEnum.InProgress);
        }

        public Task MarkEventAsFailedAsync(Guid eventId)
        {
            return UpdateEventStatus(eventId, EventStateEnum.PublishedFailed);
        }

        private Task UpdateEventStatus(Guid eventId, EventStateEnum status)
        {
            var eventLogEntry = _eventLogContext.EventLogs.Single(ie => ie.EventId == eventId);
            eventLogEntry.State = status;

            if(status == EventStateEnum.InProgress)
                eventLogEntry.TimesSent++;

            _eventLogContext.EventLogs.Update(eventLogEntry);

            return _eventLogContext.SaveChangesAsync();
        }
    }
}
