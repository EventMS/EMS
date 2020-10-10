using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EMS.Permission_Services.API.Context.Model;

namespace EMS.Permission_Services.API.Context.EntityConfigurations
{
    class UserPermissionEntityTypeConfiguration
        : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasKey(ci => ci.UserId);

            builder.Property(ci => ci.UserId)
                .IsRequired()
                .ValueGeneratedNever();
        }
    }
}
