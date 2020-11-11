using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;

namespace EMS.Club_Service.API.Context.EntityConfigurations
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
                .HasMaxLength(25)
                .IsRequired();

            builder.Property(ci => ci.AccountNumber)
                .HasMaxLength(8)
                .IsRequired();

            builder.Property(ci => ci.RegistrationNumber)
                .HasMaxLength(4)
                .IsRequired();

            builder.Property(ci => ci.Description)
                .HasMaxLength(500);

            builder.Property(ci => ci.Address)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(ci => ci.AdminId)
                .IsRequired();

            builder.Property(ci => ci.PhoneNumber)
                .HasMaxLength(8)
                .IsRequired();
        }
    }
}
