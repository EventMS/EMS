using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using EMS.TemplateWebHost.Customization.Context;

namespace EMS.Websocket_Services.API.Context
{
    public class WebsocketContext : BaseContext
    {
        public WebsocketContext(DbContextOptions<WebsocketContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
        }
    }


    public class WebsocketContextDesignFactory : IDesignTimeDbContextFactory<WebsocketContext>
    {
        public WebsocketContext CreateDbContext(string[] args)
        {
            var optionsBuilder =  new DbContextOptionsBuilder<WebsocketContext>()
                .UseSqlServer("Server=.;Initial Catalog=Services.WebsocketDb;Integrated Security=true");

            return new WebsocketContext(optionsBuilder.Options);
        }
    }
}
