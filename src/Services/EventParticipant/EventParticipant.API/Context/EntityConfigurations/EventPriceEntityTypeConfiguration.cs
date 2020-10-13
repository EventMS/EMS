using EMS.EventParticipant_Services.API.Context.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.EventParticipant_Services.API.Context.EntityConfigurations
{
    class EventPriceEntityTypeConfiguration
        : IEntityTypeConfiguration<EventPrice>
    {
        public void Configure(EntityTypeBuilder<EventPrice> builder)
        {
            builder.ToTable("EventPrice");

            builder.HasKey(ci => new {ci.EventId, ci.SubscriptionId});

            builder.HasOne<Event>()
                .WithMany(e => e.EventPrices)
                .HasForeignKey(e => e.EventId);

            builder.Property(ci => ci.SubscriptionId)
                .IsRequired();
        }
    }
}