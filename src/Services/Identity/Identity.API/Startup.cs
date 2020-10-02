using System;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using EMS.Events;
using GreenPipes;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Interceptors;
using HotChocolate.Execution.Configuration;
using Identity.API.Context.Models;
using Identity.API.Data;
using Identity.API.GraphQlQueries;
using Identity.API.Services;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using TemplateWebHost.Customization.StartUp;

namespace Identity.API
{
    public class Startup : BaseStartUp<ApplicationDbContext>
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void ConfigureMassTransit(IServiceCollectionBusConfigurator busServices)
        {
            //busServices.AddConsumer<ValueEnteredEvent2Consumer>();
        }

        public override IServiceCollection AddServices(IServiceCollection service)
        {
            service.AddSingleton<JwtService>();
            return service;
        }

        public override ISchemaBuilder AddGraphQlServices(ISchemaBuilder builder)
        {
            return builder.AddQueryType<IdentityQueries>()
                .AddMutationType<IdentityMutations>();
        }

        private static OnCreateRequestAsync AuthenticationInterceptor()
        {
            return (context, builder, token) =>
            {
                if (context.GetUser().Identity.IsAuthenticated)
                {
                    builder.SetProperty("currentUser",
                        new CurrentUser(context.User.FindFirstValue("id")));
                }

                return Task.CompletedTask;
            };
        }

        public override IServiceCollection AddGlobalStateInterceptor(IServiceCollection service)
        {
            service.AddQueryRequestInterceptor(AuthenticationInterceptor());
            return service;
        }

        public override IServiceCollection AddIdentityServer(IServiceCollection service)
        {
            service.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            return service;
        }


        protected override string GetName()
        {
            return "Identity";
        }
    }
}