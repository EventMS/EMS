using HotChocolate;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.Configuration;
using EMS.ClubMember_Services.API.Context;
using EMS.ClubMember_Services.API.GraphQlQueries;
using EMS.ClubMember_Services.API.Events;
using EMS.TemplateWebHost.Customization.StartUp;



namespace EMS.ClubMember_Services.API
{
    public class Startup : BaseStartUp<ClubMemberContext>
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void ConfigureMassTransit(IServiceCollectionBusConfigurator busServices)
        {
            busServices.AddConsumer<ClubSubscriptionCreatedEventConsumer>();
            busServices.AddConsumer<IsUserClubMemberEventConsumer>();
        }

        public override ISchemaBuilder AddGraphQlServices(ISchemaBuilder builder)
        {
            return builder
                .AddQueryType<ClubMemberQueries>()
                .AddMutationType<ClubMemberMutations>();
        }

        protected override string GetName()
        {
            return "ClubMember";
        }
    }
}
