using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EMS.Permission_Services.API.Context.Model;

namespace EMS.Permission_Services.API.Context.EntityConfigurations
{
    class UserPermissionEntityTypeConfiguration
        : IEntityTypeConfiguration<UserPermission>
    {
        public void Configure(EntityTypeBuilder<UserPermission> builder)
        {
            builder.ToTable("UserPermission");

            builder.HasKey(ci => ci.UserId);

            builder.Property(ci => ci.UserId)
                .IsRequired()
                .ValueGeneratedNever();
        }
    }
}
