
using EMS.Payment_Services.API.Context.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Payment_Services.API.Context.EntityConfigurations
{
    class EventPriceEntityTypeConfiguration
        : IEntityTypeConfiguration<EventPrice>
    {
        public void Configure(EntityTypeBuilder<EventPrice> builder)
        {
            builder.ToTable("EventPrice");

            builder.HasKey(ci => new {ci.EventId, ci.ClubSubscriptionId});

            builder.HasOne(e => e.Event)
                .WithMany(e => e.EventPrices)
                .HasForeignKey(e => e.EventId)
                .IsRequired();

            builder.Property(ci => ci.ClubSubscriptionId)
                .IsRequired();
        }
    }
}