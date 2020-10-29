using EMS.BuildingBlocks.EventLogEF;
using Microsoft.AspNetCore.Hosting;
using EMS.Permission_Services.API.Context;
using EMS.TemplateWebHost.Customization;
using EMS.TemplateWebHost.Customization.StartUp;

namespace EMS.Permission_Services.API
{

    public class ProgramHelper : BaseProgramHelper<Startup>
    {
        public ProgramHelper(string appName) : base(appName)
        {
        }

        public override void MigrateDbContext(IWebHost host)
        {
            host.MigrateDbContext<PermissionContext>((context, services) => {})
                .MigrateDbContext<EventLogContext>((context, services) => { });
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
