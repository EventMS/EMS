using EMS.BuildingBlocks.IntegrationEventLogEF;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Permission.API.Context;
using TemplateWebHost.Customization;
using TemplateWebHost.Customization.Settings;
using TemplateWebHost.Customization.StartUp;

namespace Permission.API
{

    public class ProgramHelper : BaseProgramHelper<Startup>
    {
        public ProgramHelper(string appName) : base(appName)
        {
        }

        public override void MigrateDbContext(IWebHost host)
        {
            host.MigrateDbContext<PermissionContext>((context, services) => {})
                .MigrateDbContext<IntegrationEventLogContext>((context, services) => { });
        }
    }

    public class Program
    {

        public static string AppName = "Permission.API";
        public static int Main(string[] args)
        {
            return new ProgramHelper(AppName).Run(args);
        }
    }
}
