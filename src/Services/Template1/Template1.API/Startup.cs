using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using EMS.Events;
using HotChocolate;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template1.API.Context;
using Template1.API.GraphQlQueries;
using Template1.API.IntegrationEvents;
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
            busServices.AddConsumer<Template1CreatedIntegrationEventConsumer>();
        }

        public override IServiceCollection AddGraphQlServices(IServiceCollection services)
        {
            return services.AddGraphQL(s => SchemaBuilder.New()
                .AddServices(s)
                .AddAuthorizeDirectiveType()
                .AddQueryType<Template1Queries>()
                .AddMutationType<Template1Mutations>()
                .Create()
            );
        }

        protected override string GetName()
        {
            return "Template1";
        }
    }
}
