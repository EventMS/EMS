using HotChocolate;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.Configuration;
using EMS.PaymentWebhook_Services.API.Context;
using EMS.PaymentWebhook_Services.API.GraphQlQueries;
using EMS.PaymentWebhook_Services.API.Events;
using EMS.TemplateWebHost.Customization.StartUp;



namespace EMS.PaymentWebhook_Services.API
{
    public class Startup : BaseStartUp<PaymentWebhookContext>
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        public override ISchemaBuilder AddGraphQlServices(ISchemaBuilder builder)
        {
            return builder
                .AddQueryType<PaymentWebhookQueries>();
        }

        protected override string GetName()
        {
            return "PaymentWebhook";
        }
    }
}
