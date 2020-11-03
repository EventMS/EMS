using System;
using System.Linq;
using AutoMapper.Internal;
using HotChocolate;
using Microsoft.Extensions.Configuration;
using EMS.Event_Services.API.Context;
using EMS.Event_Services.API.GraphQlQueries;
using EMS.Event_Services.API.Events;
using EMS.TemplateWebHost.Customization.StartUp;
using MassTransit;
using HotChocolate.Types;
using EMS.Event_Services.API.Context.Model;

namespace EMS.Event_Services.API
{
    public class Startup : BaseStartUp<EventContext>
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }


        public override ISchemaBuilder AddGraphQlServices(ISchemaBuilder builder)
        {
            return builder
                .AddQueryType<EventQueries>()
                .AddMutationType<EventMutations>();
        }

        protected override string GetName()
        {
            return "Event";
        }
    }
}
