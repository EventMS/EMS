using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.BuildingBlocks.EventLogEF
{
    /// <summary>
    /// Simple EventLogContext that are used in most services. 
    /// Based on: https://github.com/dotnet-architecture/eShopOnContainers
    /// </summary>
    public class EventLogContext : DbContext
    {       
        public EventLogContext(DbContextOptions<EventLogContext> options) : base(options)
        {
        }

        public DbSet<EventLogEntry> EventLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {          
            builder.Entity<EventLogEntry>(ConfigureEventLogEntry);
        }

        void ConfigureEventLogEntry(EntityTypeBuilder<EventLogEntry> builder)
        {
            builder.ToTable("EventLog");

            builder.HasKey(e => e.EventId);

            builder.Property(e => e.EventId)
                .IsRequired();

            builder.Property(e => e.Content)
                .IsRequired();

            builder.Property(e => e.CreationTime)
                .IsRequired();

            builder.Property(e => e.State)
                .IsRequired();

            builder.Property(e => e.TimesSent)
                .IsRequired();

            builder.Property(e => e.EventTypeName)
                .IsRequired();

        }
    }
}
