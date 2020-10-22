using EMS.Club_Service_Services.API;
using HotChocolate;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.Configuration;
using EMS.Payment_Services.API.Context;
using EMS.Payment_Services.API.GraphQlQueries;
using EMS.Payment_Services.API.Events;
using EMS.TemplateWebHost.Customization.StartUp;
using Microsoft.Extensions.DependencyInjection;


namespace EMS.Payment_Services.API
{
    public class Startup : BaseStartUp<PaymentContext>
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        public override IServiceCollection AddServices(IServiceCollection service)
        {
            service.AddSingleton<StripeService>();
            return base.AddServices(service);
        }

        public override ISchemaBuilder AddGraphQlServices(ISchemaBuilder builder)
        {
            return builder
                .AddQueryType<PaymentQueries>()
                .AddMutationType<PaymentMutations>();
        }

        protected override string GetName()
        {
            return "Payment";
        }
    }
}
