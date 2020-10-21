using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Subscription_Services.API.Context.EntityConfigurations
{
    class ClubSubscriptionEntityTypeConfiguration
        : IEntityTypeConfiguration<ClubSubscription>
    {
        public void Configure(EntityTypeBuilder<ClubSubscription> builder)
        {
            builder.ToTable("ClubSubscription")
                .HasIndex(ci => new {ci.Name,ci.ClubId})
                .IsUnique();

            builder.HasKey(ci => ci.ClubSubscriptionId);

            builder.Property(ci => ci.ClubSubscriptionId)
                .IsRequired();

            builder.Property(ci => ci.StribePriceId)
            .IsRequired();

            builder.Property(ci => ci.StribeProductId)
            .IsRequired();

            builder.Property(ci => ci.Name)
                .HasMaxLength(25)
                .IsRequired();

            builder.HasOne(subscription => subscription.Club)
                .WithMany(club => club.Subscriptions)
                .HasForeignKey(ci => ci.ClubId)
                .IsRequired();

            builder.Property(ci => ci.Price)
                .IsRequired()
                .HasDefaultValue(0);
        }
    }
}
