using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using EMS.Events;
using HotChocolate;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Club.API.Context;
using Club.API.Controllers.Request;
using Club.API.GraphQlQueries;
using Club.API.IntegrationEvents;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Interceptors;
using Identity.API;
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

        private static OnCreateRequestAsync AuthenticationInterceptor()
        {
            return (context, builder, token) =>
            {
                if (context.GetUser().Identity.IsAuthenticated)
                {
                    builder.SetProperty("currentUser",
                        new CurrentUser(new Guid(context.User.FindFirstValue("id"))));
                }

                return Task.CompletedTask;
            };
        }

        public override IServiceCollection AddGlobalStateInterceptor(IServiceCollection service)
        {
            service.AddQueryRequestInterceptor(AuthenticationInterceptor());
            return service;
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
