using HotChocolate;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.Configuration;
using EMS.Subscription_Services.API.Context;
using EMS.Subscription_Services.API.GraphQlQueries;
using EMS.Subscription_Services.API.IntegrationEvents;
using EMS.TemplateWebHost.Customization.StartUp;
using Microsoft.Extensions.DependencyInjection;

namespace EMS.Subscription_Services.API
{
    public class Startup : BaseStartUp<SubscriptionContext>
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        public override ISchemaBuilder AddGraphQlServices(ISchemaBuilder builder)
        {
            return builder
                .AddQueryType<SubscriptionQueries>()
                .AddMutationType<SubscriptionMutations>();
        }

        public override IServiceCollection AddServices(IServiceCollection service)
        {
            services.AddTransient<StripeService>();
            return base.AddServices(service);
        }

        protected override string GetName()
        {
            return "Subscription";
        }
    }
}
