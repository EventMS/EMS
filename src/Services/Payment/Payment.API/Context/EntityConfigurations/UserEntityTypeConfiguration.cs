using EMS.Payment_Services.API.Context.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Payment_Services.API.Context.EntityConfigurations
{
    class UserEntityTypeConfiguration
        : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasKey(ci => ci.UserId);

            builder.Property(ci => ci.UserId)
                .IsRequired();

            builder.Property(ci => ci.StripeUserId)
                .IsRequired();
        }
    }
}
