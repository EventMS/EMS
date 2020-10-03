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
using Permission.API.Events;
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
            busServices.AddConsumer<UserCreatedEventConsumer>();
            busServices.AddConsumer<ClubCreatedEventConsumer>();
        }

        public override IServiceCollection AddServices(IServiceCollection service)
        {
            service.AddSingleton<JwtService>();
            return service;
        }

        public override ISchemaBuilder AddGraphQlServices(ISchemaBuilder builder)
        {
            return builder
                .AddQueryType<PermissionQueries>();
          
        }

        protected override string GetName()
        {
            return "Permission";
        }
    }
}
