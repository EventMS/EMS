using HotChocolate;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.Configuration;
using EMS.Template1_Services.API.Context;
using EMS.Template1_Services.API.GraphQlQueries;
using EMS.Template1_Services.API.Events;
using EMS.TemplateWebHost.Customization.StartUp;



namespace EMS.Template1_Services.API
{
    public class Startup : BaseStartUp<Template1Context>
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
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
