using System;
using System.Linq;
using AutoMapper.Internal;
using HotChocolate;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.Configuration;
using EMS.Event_Services.API.Context;
using EMS.Event_Services.API.GraphQlQueries;
using EMS.Event_Services.API.Events;
using EMS.TemplateWebHost.Customization.StartUp;
using MassTransit;
using Serilog;


namespace EMS.Event_Services.API
{
    public class Startup : BaseStartUp<EventContext>
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        public override void ConfigureMassTransit(IServiceCollectionBusConfigurator busServices)
        {
            Log.Information("Type getter tester");
            var type = typeof(IConsumer);
            AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p))
                .ForAll(p => Log.Information(p.ToString()));



            busServices.AddConsumer<ClubCreatedEventConsumer>();
            busServices.AddConsumer<ClubSubscriptionCreatedEventConsumer>();
            busServices.AddConsumer<InstructorAddedEventConsumer>();
            busServices.AddConsumer<RoomCreatedEventConsumer>();
            busServices.AddConsumer<TimeslotReservationFailedEventConsumer>();
            busServices.AddConsumer<TimeslotReservedEventConsumer>();
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
