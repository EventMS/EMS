using HotChocolate;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template1.API.Context;
using Template1.API.GraphQlQueries;
using Template1.API.Events;
using TemplateWebHost.Customization.StartUp;



namespace Template1.API
{
    public class Startup : BaseStartUp<Template1Context>
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void ConfigureMassTransit(IServiceCollectionBusConfigurator busServices)
        {
            busServices.AddConsumer<Template1CreatedEventConsumer>();
        }

        public override ISchemaBuilder AddGraphQlServices(ISchemaBuilder builder)
        {
            return builder
                .AddQueryType<Template1Queries>()
                .AddMutationType<Template1Mutations>();
        }

        protected override string GetName()
        {
            return "Template1";
        }
    }
}
