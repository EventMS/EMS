using EMS.EventParticipant_Services.API.Context.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.EventParticipant_Services.API.Context.EntityConfigurations
{
    class EventEntityTypeConfiguration
        : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable("Event");

            builder.HasKey(ci => ci.EventId);

            builder.Property(ci => ci.EventType)
                .IsRequired();

            builder.Property(ci => ci.ClubId)
                .IsRequired();

            builder.Property(ci => ci.IsFree)
                .IsRequired();
        }
    }
}