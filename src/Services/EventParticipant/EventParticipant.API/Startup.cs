using HotChocolate;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.Configuration;
using EMS.EventParticipant_Services.API.Context;
using EMS.EventParticipant_Services.API.GraphQlQueries;
using EMS.EventParticipant_Services.API.Events;
using EMS.TemplateWebHost.Customization.StartUp;



namespace EMS.EventParticipant_Services.API
{
    public class Startup : BaseStartUp<EventParticipantContext>
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        public override ISchemaBuilder AddGraphQlServices(ISchemaBuilder builder)
        {
            return builder
                .AddQueryType<EventParticipantQueries>()
                .AddMutationType<EventParticipantMutations>();
        }

        protected override string GetName()
        {
            return "EventParticipant";
        }
    }
}
