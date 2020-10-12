using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using EMS.EventParticipant_Services.API.Context.EntityConfigurations;
using EMS.TemplateWebHost.Customization.Context;

namespace EMS.EventParticipant_Services.API.Context
{
    using Model;
    public class EventParticipantContext : BaseContext
    {
        public EventParticipantContext(DbContextOptions<EventParticipantContext> options) : base(options)
        {
        }
        public DbSet<EventParticipant> EventParticipants { get; set; }
        public DbSet<Event> Events { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new EventParticipantEntityTypeConfiguration());
            builder.ApplyConfiguration(new EventEntityTypeConfiguration());
        }
    }


    public class EventParticipantContextDesignFactory : IDesignTimeDbContextFactory<EventParticipantContext>
    {
        public EventParticipantContext CreateDbContext(string[] args)
        {
            var optionsBuilder =  new DbContextOptionsBuilder<EventParticipantContext>()
                .UseSqlServer("Server=.;Initial Catalog=Services.EventParticipantDb;Integrated Security=true");

            return new EventParticipantContext(optionsBuilder.Options);
        }
    }
}
