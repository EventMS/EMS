using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Subscription_Services.API.Context.EntityConfigurations
{
    class ClubEntityTypeConfiguration
        : IEntityTypeConfiguration<Club>
    {
        public void Configure(EntityTypeBuilder<Club> builder)
        {
            builder.ToTable("Club");

            builder.HasKey(ci => ci.ClubId);

            builder.Property(ci => ci.ClubId)
                .IsRequired();
        }
    }
}