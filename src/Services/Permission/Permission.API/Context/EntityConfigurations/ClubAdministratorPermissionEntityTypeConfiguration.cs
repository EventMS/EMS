using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Permission.API.Context.Model;

namespace Permission.API.Context.EntityConfigurations
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