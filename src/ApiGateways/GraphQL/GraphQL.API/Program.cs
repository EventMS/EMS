using EMS.TemplateWebHost.Customization.StartUp;

namespace EMS.GraphQL.API
{
    public class Program
    {
        public static string AppName = "GraphQL.API";
        public static int Main(string[] args)
        {
            return new BaseProgramHelper<Startup>(AppName).Run(args);
        }
    }
}
