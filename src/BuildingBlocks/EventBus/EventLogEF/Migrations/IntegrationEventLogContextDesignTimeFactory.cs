using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EMS.BuildingBlocks.EventLogEF.Migrations
{
    public class EventLogContextDesignTimeFactory : IDesignTimeDbContextFactory<EventLogContext>
    {
        public EventLogContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EventLogContext>();

            optionsBuilder.UseSqlServer(".", options => options.MigrationsAssembly(GetType().Assembly.GetName().Name));

            return new EventLogContext(optionsBuilder.Options);
        }
    }
}
