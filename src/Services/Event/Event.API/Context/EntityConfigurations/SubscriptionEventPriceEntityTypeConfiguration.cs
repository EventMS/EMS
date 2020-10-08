using EMS.Event_Services.API.Context.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Event_Services.API.Context.EntityConfigurations
{
    class SubscriptionEventPriceEntityTypeConfiguration
        : IEntityTypeConfiguration<ClubSubscriptionEventPrice>
    {
        public void Configure(EntityTypeBuilder<ClubSubscriptionEventPrice> builder)
        {
            builder.ToTable("ClubSubscriptionEventPrice");

            builder.HasKey(ci => new { ci.EventId, ci.SubscriptionId });

            builder.HasOne(bc => bc.Event)
                .WithMany(b => b.SubscriptionEventPrices)
                .HasForeignKey(bc => bc.EventId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(bc => bc.ClubSubscription)
                .WithMany(c => c.ClubSubscriptionEventPrices)
                .HasForeignKey(bc => bc.SubscriptionId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}