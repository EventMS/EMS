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
using Permission.API.Context;
using Permission.API.GraphQlQueries;
using Permission.API.IntegrationEvents;
using Permission.API.Services;
using TemplateWebHost.Customization.StartUp;



namespace Permission.API
{
    public class Startup : BaseStartUp<PermissionContext>
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void ConfigureMassTransit(IServiceCollectionBusConfigurator busServices)
        {
            busServices.AddConsumer<UserCreatedEventPermissionConsumer>();
            busServices.AddConsumer<ClubCreatedIntegrationEventPermissionConsumer>();
        }

        public override IServiceCollection AddServices(IServiceCollection service)
        {
            service.AddSingleton<JwtService>();
            return service;
        }

        public override IServiceCollection AddGraphQlServices(IServiceCollection services)
        {
            return services.AddGraphQL(s => SchemaBuilder.New()
                .AddServices(s)
                .AddAuthorizeDirectiveType()
                .AddQueryType<PermissionQueries>()
                .Create()
            );
        }

        protected override string GetName()
        {
            return "UserPermission";
        }
    }
}
