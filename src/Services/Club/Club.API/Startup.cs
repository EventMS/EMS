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
using Club.API.Events;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Interceptors;
using Identity.API;
using TemplateWebHost.Customization.StartUp;
using Club.API.Context.Model;
using TemplateWebHost.Customization.Filters;

namespace Club.API
{
    public class Startup : BaseStartUp<ClubContext>
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void ConfigureMassTransit(IServiceCollectionBusConfigurator busServices)
        {
            busServices.AddConsumer<UserIsClubMemberClubConsumer>();
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
