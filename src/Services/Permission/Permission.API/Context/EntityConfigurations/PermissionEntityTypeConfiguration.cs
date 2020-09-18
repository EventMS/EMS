using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Permission.API.Context.EntityConfigurations
{
    class PermissionEntityTypeConfiguration
        : IEntityTypeConfiguration<Model.Permission>
    {
        public void Configure(EntityTypeBuilder<Model.Permission> builder)
        {
            builder.ToTable("Permission");

            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id)
                .IsRequired();

            builder.Property(ci => ci.Name)
                .IsRequired();
        }
    }
}
