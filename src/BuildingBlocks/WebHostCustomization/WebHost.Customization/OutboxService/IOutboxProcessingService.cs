using System.Threading;
using System.Threading.Tasks;

namespace TemplateWebHost.Customization.OutboxService
{
    internal interface IOutboxProcessingService
    {
        Task DoWork(CancellationToken stoppingToken);
    }
}