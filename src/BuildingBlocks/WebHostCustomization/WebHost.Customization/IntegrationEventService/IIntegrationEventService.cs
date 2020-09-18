using System;
using System.Threading.Tasks;
using EMS.BuildingBlocks.IntegrationEventLogEF;

namespace TemplateWebHost.Customization.IntegrationEvents
{
    public interface IIntegrationEventService
    {
        Task SaveEventAndDbContextChangesAsync(IntegrationEvent evt, Func<Task> action = null);
        Task PublishThroughEventBusAsync<T>(T evt) where T : IntegrationEvent;
        Task SaveContextThenPublishEvent(IntegrationEvent evt, Func<Task> action = null);
    }
}
