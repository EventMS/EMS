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
using Club.API.Context;
using Club.API.GraphQlQueries;
using Club.API.IntegrationEvents;
using TemplateWebHost.Customization.StartUp;



namespace Club.API
{
    public class Startup : BaseStartUp<ClubContext>
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void ConfigureMassTransit(IServiceCollectionBusConfigurator busServices)
        {
            busServices.AddConsumer<UserCreatedIntegrationEventClubConsumer>();
        }

        public override IServiceCollection AddGraphQlServices(IServiceCollection services)
        {
            return services.AddGraphQL(s => SchemaBuilder.New()
                .AddServices(s)
                .AddAuthorizeDirectiveType()
                .AddQueryType<ClubQueries>()
                .AddMutationType<ClubMutations>()
                .Create()
            );
        }

        protected override string GetName()
        {
            return "Club";
        }
    }
}
