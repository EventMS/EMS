using EMS.Event_Services.API.Context.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Event_Services.API.Context.EntityConfigurations
{
    class EventEntityTypeConfiguration
        : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable("Event");

            builder.HasKey(ci => ci.EventId);

            builder.Property(ci => ci.EventId)
                .IsRequired();

            builder.Property(ci => ci.Name)
                .IsRequired();

            builder.Property(ci => ci.Description)
                .IsRequired();

            builder.Property(ci => ci.EndTime)
                .IsRequired();

            builder.Property(ci => ci.StartTime)
                .IsRequired();
        }
    }
}
