using HotChocolate;
using EMS.Identity_Services.API.Context.Models;
using EMS.Identity_Services.API.Data;
using EMS.Identity_Services.API.GraphQlQueries;
using EMS.Identity_Services.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EMS.TemplateWebHost.Customization.StartUp;

namespace EMS.Identity_Services.API
{
    public class Startup : BaseStartUp<ApplicationDbContext>
    {
        public Startup(IConfiguration configuration) : base(configuration)
        {
        }


        public override IServiceCollection AddServices(IServiceCollection service)
        {
            service.AddSingleton<JwtService>();
            return service;
        }

        public override ISchemaBuilder AddGraphQlServices(ISchemaBuilder builder)
        {
            return builder.AddQueryType<IdentityQueries>()
                .AddMutationType<IdentityMutations>();
        }

        public override IServiceCollection AddIdentityServer(IServiceCollection service)
        {
            service.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            return service;
        }


        protected override string GetName()
        {
            return "Identity";
        }
    }
}