using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EMS.Permission_Services.API.Context.Model;

namespace EMS.Permission_Services.API.Context.EntityConfigurations
{
    class ClubAdministratorPermissionEntityTypeConfiguration
        : IEntityTypeConfiguration<ClubAdministratorPermission>
    {
        public void Configure(EntityTypeBuilder<ClubAdministratorPermission> builder)
        {
            builder.ToTable("ClubAdministratorPermission");

            builder.HasKey(ci => ci.ClubId);

            builder.Property(ci => ci.ClubId)
                .IsRequired()
                .ValueGeneratedNever();
        }
    }
}