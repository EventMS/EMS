using System;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using EMS.Events;
using GreenPipes;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Interceptors;
using Identity.API.Data;
using Identity.API.GraphQlQueries;
using Identity.API.Services;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.eShopOnContainers.Services.Identity.API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using TemplateWebHost.Customization.StartUp;

namespace Identity.API
{

    class ValueEnteredEvent2Consumer :
        IConsumer<ValueEntered>
    {
        public async Task Consume(ConsumeContext<ValueEntered> context)
        {
            Log.Information("IdentityValue: {Value}", context.Message.Value);
            Log.Information("IdentityValue2: {Value2}", context.Message.Value2);
        }
    }
    public class CurrentUser
    {
        public string UserId { get; }

        public CurrentUser(string userId)
        {
            UserId = userId;
        }
    }

    public class CurrentUserGlobalState : GlobalStateAttribute
    {
        public CurrentUserGlobalState() : base("currentUser")
        {
        }
    }


    public class Startup : BaseStartUp<ApplicationDbContext>
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void ConfigureMassTransit(IServiceCollectionBusConfigurator busServices)
        {
            busServices.AddConsumer<ValueEnteredEvent2Consumer>();
        }

        public override IServiceCollection AddGraphQlServices(IServiceCollection services)
        {
            services.AddSingleton<JwtService>();
            //Services
            return services.AddGraphQL(s => SchemaBuilder.New()
                .AddServices(s)
                .AddQueryType<Query>()
                .AddMutationType<MutationQuery>()
                .AddAuthorizeDirectiveType()
                .Create()
            );
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

        public override IServiceCollection AddIdentityServer(IServiceCollection service)
        {
            service.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            return service;
        }

        public override IServiceCollection AddEventBusHandlers(IServiceCollection services)
        {
            services.AddQueryRequestInterceptor(AuthenticationInterceptor());
            return services;
        }

        protected override string GetName()
        {
            return "Identity";
        }
    }
}