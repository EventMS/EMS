using EMS.EventParticipant_Services.API.Context.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.EventParticipant_Services.API.Context.EntityConfigurations
{
    class EventParticipantEntityTypeConfiguration
        : IEntityTypeConfiguration<EventParticipant>
    {
        public void Configure(EntityTypeBuilder<EventParticipant> builder)
        {
            builder.ToTable("EventParticipant");

            builder.HasKey(ci => ci.EventParticipantId);

            builder.Property(ci => ci.EventParticipantId)
                .IsRequired();

            builder.Property(ci => ci.UserId)
                .IsRequired();

            builder.HasOne<Event>()
                .WithMany(e => e.EventParticipants)
                .HasForeignKey(e => e.EventId)
                .IsRequired();
        }
    }
}
