using EMS.Room_Services.API.Context.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Room_Services.API.Context.EntityConfigurations
{
    class ClubEntityTypeConfiguration
        : IEntityTypeConfiguration<Club>
    {
        public void Configure(EntityTypeBuilder<Club> builder)
        {
            builder.ToTable("Club");

            builder.HasKey(ci => ci.ClubId);
        }
    }
}