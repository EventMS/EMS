using EMS.Payment_Services.API.Context.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Payment_Services.API.Context.EntityConfigurations
{
    class ClubSubscriptionEntityTypeConfiguration
        : IEntityTypeConfiguration<ClubSubscription>
    {
        public void Configure(EntityTypeBuilder<ClubSubscription> builder)
        {
            builder.ToTable("ClubSubscription");

            builder.HasKey(ci => ci.ClubSubscriptionId);

            builder.Property(ci => ci.StripePriceId)
                .IsRequired();

            builder.Property(ci => ci.ClubId)
                .IsRequired();

            builder.Property(ci => ci.StripeProductId)
                .IsRequired();
        }
    }
}