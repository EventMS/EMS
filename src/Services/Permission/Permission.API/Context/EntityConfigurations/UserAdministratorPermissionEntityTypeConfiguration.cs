using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EMS.Permission_Services.API.Context.Model;

namespace EMS.Permission_Services.API.Context.EntityConfigurations
{
    class UserAdministratorPermissionEntityTypeConfiguration
        : IEntityTypeConfiguration<UserAdministratorPermission>
    {
        public void Configure(EntityTypeBuilder<UserAdministratorPermission> builder)
        {
            builder.ToTable("UserAdministratorPermission");

            builder.HasKey(ci => new {ci.UserId, ci.ClubId});

            builder.HasOne(bc => bc.UserPermission)
                .WithMany(b => b.ClubAdminIn)
                .HasForeignKey(bc => bc.UserId);
            builder.HasOne(bc => bc.ClubAdministratorPermission)
                .WithMany(c => c.Users)
                .HasForeignKey(bc => bc.ClubId);
        }
    }
}