using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EMS.Permission_Services.API.Context.Model;

namespace EMS.Permission_Services.API.Context.EntityConfigurations
{
    class RoleEntityTypeConfiguration
        : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");

            builder.HasKey(ci => new {ci.UserId, ci.ClubId});

            builder.HasOne(bc => bc.User)
                .WithMany(b => b.Roles)
                .HasForeignKey(bc => bc.UserId);
            builder.HasOne(bc => bc.Club)
                .WithMany(c => c.Users)
                .HasForeignKey(bc => bc.ClubId);
        }
    }
}