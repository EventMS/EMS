using EMS.ClubMember_Services.API.Context.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.ClubMember_Services.API.Context.EntityConfigurations
{
    class ClubSubscriptionEntityTypeConfiguration
        : IEntityTypeConfiguration<ClubSubscription>
    {
        public void Configure(EntityTypeBuilder<ClubSubscription> builder)
        {
            builder.ToTable("ClubSubscription");

            builder.HasKey(ci => new {ci.ClubSubscriptionId});
        }
    }
}