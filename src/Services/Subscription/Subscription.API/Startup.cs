using HotChocolate;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.Configuration;
using EMS.Subscription_Services.API.Context;
using EMS.Subscription_Services.API.GraphQlQueries;
using EMS.Subscription_Services.API.IntegrationEvents;
using EMS.TemplateWebHost.Customization.StartUp;



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

        protected override string GetName()
        {
            return "Subscription";
        }
    }
}
