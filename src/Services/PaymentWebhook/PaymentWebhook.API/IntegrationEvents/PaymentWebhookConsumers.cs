using System.Threading.Tasks;
using EMS.Events;
using MassTransit;
using Serilog;

namespace EMS.PaymentWebhook_Services.API.Events
{
    public class PaymentWebhookCreatedEventConsumer :
            IConsumer<PaymentWebhookCreatedEvent>
        {
            public async Task Consume(ConsumeContext<PaymentWebhookCreatedEvent> context)
            {
                Log.Information("PaymentWebhookValue: {Value}", context.Message.Name);
                Log.Information("PaymentWebhookValue: {Value}", context.Message.CreationDate);
                Log.Information("PaymentWebhookValue: {Value}", context.Message.Id);
            }
        }
    }
