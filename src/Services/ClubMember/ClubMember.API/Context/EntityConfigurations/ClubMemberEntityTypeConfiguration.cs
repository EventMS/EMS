using EMS.ClubMember_Services.API.Context.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.ClubMember_Services.API.Context.EntityConfigurations
{
    class ClubMemberEntityTypeConfiguration
        : IEntityTypeConfiguration<ClubMember>
    {
        public void Configure(EntityTypeBuilder<ClubMember> builder)
        {
            builder.ToTable("ClubMember");

            builder.HasKey(ci => new {ci.UserId, ci.ClubId});

            builder.HasOne<ClubSubscription>()
                .WithMany(ci => ci.ClubMembers)
                .HasForeignKey(ci => new {ci.ClubSubscriptionId})
                .IsRequired();

            builder.Property(ci => ci.ClubSubscriptionId).IsRequired();
        }
    }
}
