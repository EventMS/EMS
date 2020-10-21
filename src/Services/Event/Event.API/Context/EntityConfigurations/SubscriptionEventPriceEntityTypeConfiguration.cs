using EMS.Event_Services.API.Context.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Event_Services.API.Context.EntityConfigurations
{
    class SubscriptionEventPriceEntityTypeConfiguration
        : IEntityTypeConfiguration<EventPrice>
    {
        public void Configure(EntityTypeBuilder<EventPrice> builder)
        {
            builder.ToTable("EventPrice");

            builder.HasKey(ci => new { ci.EventId, ci.ClubSubscriptionId });

            builder.HasOne(bc => bc.Event)
                .WithMany(b => b.EventPrices)
                .HasForeignKey(bc => bc.EventId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(bc => bc.ClubSubscription)
                .WithMany(c => c.EventPrices)
                .HasForeignKey(bc => bc.ClubSubscriptionId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}