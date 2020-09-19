using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Permission.API.Context.Model;

namespace Permission.API.Context.EntityConfigurations
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
