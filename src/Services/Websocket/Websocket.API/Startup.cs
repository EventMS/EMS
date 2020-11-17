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
    public class UserIdProvider: IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            if(connection.User != null)
            {
                return connection.User.FindFirst(claim => claim.Type == "id").Value;
            }
            return connection.UserIdentifier;
        }
    }

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
            service.AddSingleton<IUserIdProvider, UserIdProvider>();
            service.AddSignalR();
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
