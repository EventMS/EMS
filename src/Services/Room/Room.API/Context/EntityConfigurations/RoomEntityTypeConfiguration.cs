using EMS.Room_Services.API.Context.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Room_Services.API.Context.EntityConfigurations
{
    class RoomEntityTypeConfiguration
        : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.ToTable("Room")
                .HasIndex(ci => new {ci.Name, ci.ClubId})
                .IsUnique();

            builder.HasKey(ci => ci.RoomId);

            builder.Property(ci => ci.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne<Club>()
                .WithMany(ci => ci.Rooms)
                .HasForeignKey(ci => ci.ClubId)
                .IsRequired();
        }
    }
}
