using EMS.BuildingBlocks.EventLogEF;
using Identity.API.Data;
using Microsoft.AspNetCore.Hosting;
using TemplateWebHost.Customization;
using TemplateWebHost.Customization.StartUp;

namespace Identity.API
{

    public class ProgramHelper : BaseProgramHelper<Startup>
    {
        public ProgramHelper(string appName) : base(appName)
        {
        }

        public override void MigrateDbContext(IWebHost host)
        {
            host.MigrateDbContext<ApplicationDbContext>((_, __) => {})
                .MigrateDbContext<EventLogContext>((_, __) => { });
        }
    }
    public class Program
    {

        public static string AppName = "Identity.API";
        public static int Main(string[] args)
        {
            return new ProgramHelper(AppName).Run(args);
        }
    }
}
