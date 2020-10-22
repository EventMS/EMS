using HotChocolate;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.Configuration;
using EMS.EventVerification_Services.API.Context;
using EMS.EventVerification_Services.API.GraphQlQueries;
using EMS.EventVerification_Services.API.Events;
using EMS.TemplateWebHost.Customization.StartUp;



namespace EMS.EventVerification_Services.API
{
    public class Startup : BaseStartUp<EventVerificationContext>
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        public override ISchemaBuilder AddGraphQlServices(ISchemaBuilder builder)
        {
            return builder
                .AddQueryType<EventVerificationQueries>()
                .AddMutationType<EventVerificationMutations>();
        }

        protected override string GetName()
        {
            return "EventVerification";
        }
    }
}
