using HotChocolate;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Subscription.API.Context;
using Subscription.API.GraphQlQueries;
using Subscription.API.IntegrationEvents;
using TemplateWebHost.Customization.StartUp;



namespace Subscription.API
{
    public class Startup : BaseStartUp<SubscriptionContext>
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void ConfigureMassTransit(IServiceCollectionBusConfigurator busServices)
        {
            busServices.AddConsumer<ClubCreatedEventConsumer>();
        }

        public override ISchemaBuilder AddGraphQlServices(ISchemaBuilder builder)
        {
            return builder
                .AddQueryType<SubscriptionQueries>()
                .AddMutationType<SubscriptionMutations>();
        }

        protected override string GetName()
        {
            return "Subscription";
        }
    }
}
