using EMS.BuildingBlocks.EventLogEF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using EMS.Websocket_Services.API.Context;
using EMS.TemplateWebHost.Customization;
using EMS.TemplateWebHost.Customization.Settings;
using EMS.TemplateWebHost.Customization.StartUp;

namespace EMS.Websocket_Services.API
{

    public class ProgramHelper : BaseProgramHelper<Startup>
    {
        public ProgramHelper(string appName) : base(appName)
        {
        }

        public override void MigrateDbContext(IWebHost host)
        {
            host.MigrateDbContext<WebsocketContext>((context, services) =>
                {

                })
                .MigrateDbContext<EventLogContext>((context, services) => { });
        }
    }

    public class Program
    {

        public static string AppName = "Websocket.API";
        public static int Main(string[] args)
        {
            return new ProgramHelper(AppName).Run(args);
        }
    }
}
