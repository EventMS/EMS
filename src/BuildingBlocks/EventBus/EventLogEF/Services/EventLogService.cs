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
            var result = await _eventLogContext.EventLogs
                .Where(e =>e.State == EventStateEnum.PublishedFailed).ToListAsync();

            if(result.Any()){
                return result.OrderBy(o => o.CreationTime)
                    .Select(e => e.DeserializeJsonContent(_eventTypes.Find(t=> t.Name == e.EventTypeShortName)));
            }
            
            return new List<EventLogEntry>();
        }

        public Task SaveEventAsync(Event @event, IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            var eventLogEntry = new EventLogEntry(@event, transaction.TransactionId);

            _eventLogContext.Database.UseTransaction(transaction.GetDbTransaction());
            _eventLogContext.EventLogs.Add(eventLogEntry);
            return _eventLogContext.SaveChangesAsync();
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
