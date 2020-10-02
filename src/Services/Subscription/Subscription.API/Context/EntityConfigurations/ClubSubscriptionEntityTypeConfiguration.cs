using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Subscription.API.Context.EntityConfigurations
{
    class ClubSubscriptionEntityTypeConfiguration
        : IEntityTypeConfiguration<ClubSubscription>
    {
        public void Configure(EntityTypeBuilder<ClubSubscription> builder)
        {
            builder.ToTable("ClubSubscription")
                .HasIndex(ci => ci.Name)
                .IsUnique();

            builder.HasKey(ci => ci.SubscriptionId);

            builder.Property(ci => ci.SubscriptionId)
                .IsRequired();

            builder.Property(ci => ci.Name)
                .IsRequired();

            builder.HasOne(subscription => subscription.Club)
                .WithMany(club => club.Subscriptions)
                .HasForeignKey(ci => ci.ClubId)
                .IsRequired();

            builder.Property(ci => ci.Price)
                .IsRequired();
        }
    }
}
