using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EMS.BuildingBlocks.EventLogEF;
using Microsoft.EntityFrameworkCore.Storage;

namespace EMS.BuildingBlocks.IntegrationEventLogEF.Services
{
    public interface IEventLogService
    {
        Task<IEnumerable<EventLogEntry>> RetrieveEventLogsFailedToPublishAsync();
        Task SaveEventAsync(Event @event, IDbContextTransaction transaction);
        Task MarkEventAsPublishedAsync(Guid eventId);
        Task MarkEventAsInProgressAsync(Guid eventId);
        Task MarkEventAsFailedAsync(Guid eventId);
    }
}
