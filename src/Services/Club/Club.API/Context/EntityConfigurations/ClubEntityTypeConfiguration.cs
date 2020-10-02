using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;

namespace Club.API.Context.EntityConfigurations
{
    class ClubEntityTypeConfiguration
        : IEntityTypeConfiguration<Model.Club>
    {
        public void Configure(EntityTypeBuilder<Model.Club> builder)
        {
            builder.ToTable("Club")
                .HasIndex(club => club.Name)
                .IsUnique();

            builder.HasKey(ci => ci.ClubId);

            builder.Property(ci => ci.ClubId)
                .IsRequired();

            builder.Property(ci => ci.Name)
                .IsRequired();

            builder.Property(ci => ci.AccountNumber)
                .IsRequired();

            builder.Property(ci => ci.RegistrationNumber)
                .IsRequired();

            builder.Property(ci => ci.Description);

            builder.Property(ci => ci.Address)
                .IsRequired();

            builder.Property(ci => ci.AdminId)
                .IsRequired();

            builder.Property(ci => ci.PhoneNumber)
                .IsRequired();

            builder.Property(ci => ci.InstructorIds)
                .HasConversion(v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(v => new Guid(v)).ToHashSet());

        }
    }
}
