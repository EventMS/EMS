using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using EMS.Event_Services.API.Context.EntityConfigurations;
using EMS.TemplateWebHost.Customization.Context;

namespace EMS.Event_Services.API.Context
{
    using Model;
    public class EventContext : BaseContext
    {
        public EventContext(DbContextOptions<EventContext> options) : base(options)
        {
        }
        public DbSet<Event> Events { get; set; }

        public DbSet<Club> Clubs { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<Instructor> Instructors { get; set; }

        public DbSet<Subscription> Subscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new EventEntityTypeConfiguration());
        }
    }


    public class EventContextDesignFactory : IDesignTimeDbContextFactory<EventContext>
    {
        public EventContext CreateDbContext(string[] args)
        {
            var optionsBuilder =  new DbContextOptionsBuilder<EventContext>()
                .UseSqlServer("Server=.;Initial Catalog=Services.EventDb;Integrated Security=true");

            return new EventContext(optionsBuilder.Options);
        }
    }
}
