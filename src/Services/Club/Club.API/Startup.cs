using HotChocolate;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.Configuration;
using EMS.Club_Service.API.Context;
using EMS.Club_Service.API.GraphQlQueries;
using EMS.Club_Service.API.Events;
using EMS.TemplateWebHost.Customization.StartUp;

namespace EMS.Club_Service.API
{
    public class Startup : BaseStartUp<ClubContext>
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void ConfigureMassTransit(IServiceCollectionBusConfigurator busServices)
        {
            busServices.AddConsumer<UserIsClubMemberConsumer>();
        }
        
        public override ISchemaBuilder AddGraphQlServices(ISchemaBuilder builder)
        {
            return builder.AddQueryType<ClubQueries>()
                .AddMutationType<ClubMutations>();
        }

        protected override string GetName()
        {
            return "Club";
        }
    }
}
