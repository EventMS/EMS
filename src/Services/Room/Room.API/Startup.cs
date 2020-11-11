using HotChocolate;
using Microsoft.Extensions.Configuration;
using EMS.Room_Services.API.Context;
using EMS.Room_Services.API.GraphQlQueries;
using EMS.Room_Services.API.Events;
using EMS.TemplateWebHost.Customization.StartUp;



namespace EMS.Room_Services.API
{
    public class Startup : BaseStartUp<RoomContext>
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }

        public override ISchemaBuilder AddGraphQlServices(ISchemaBuilder builder)
        {
            return builder
                .AddQueryType<RoomQueries>()
                .AddMutationType<RoomMutations>();
        }

        protected override string GetName()
        {
            return "Room";
        }
    }
}
