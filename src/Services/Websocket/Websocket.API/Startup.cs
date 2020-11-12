using HotChocolate;
using Microsoft.Extensions.Configuration;
using EMS.Websocket_Services.API.Context;
using EMS.Websocket_Services.API.Events;
using EMS.TemplateWebHost.Customization.StartUp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;


namespace EMS.Websocket_Services.API
{
    public class WebsocketQueries
    {

    }
    public class Startup : BaseStartUp<WebsocketContext>
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        public override IServiceCollection AddServices(IServiceCollection service)
        {
            service.AddSignalR(settings =>
            {
                settings.ClientTimeoutInterval = new System.TimeSpan(1,0,0);
                settings.KeepAliveInterval = new System.TimeSpan(1, 0, 0);
                settings.HandshakeTimeout = new System.TimeSpan(1, 0, 0);
				settings.EnableDetailedErrors = true;
            });
            return base.AddServices(service);
        }

        public override ISchemaBuilder AddGraphQlServices(ISchemaBuilder builder)
        {
            return builder
                .AddQueryType<WebsocketQueries>();
        }

        protected override void AddUseEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapHub<EventHub>("/event");
        }


        protected override string GetName()
        {
            return "Websocket";
        }
    }
}
