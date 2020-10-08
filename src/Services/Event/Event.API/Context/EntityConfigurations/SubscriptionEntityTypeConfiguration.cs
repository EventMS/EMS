using EMS.Event_Services.API.Context.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Event_Services.API.Context.EntityConfigurations
{
    class SubscriptionEntityTypeConfiguration
        : IEntityTypeConfiguration<ClubSubscription>
    {
        public void Configure(EntityTypeBuilder<ClubSubscription> builder)
        {
            builder.ToTable("ClubSubscription");

            builder.HasKey(ci => ci.ClubSubscriptionId);

            builder.HasOne<Club>()
                .WithMany()
                .HasForeignKey(ci => ci.ClubId);
        }
    }
}