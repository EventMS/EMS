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
                .HasIndex(ci => ci.Name)
                .IsUnique();

            builder.HasKey(ci => ci.RoomId);

            builder.Property(ci => ci.Name)
                .IsRequired();

            builder.HasOne<Club>()
                .WithMany(ci => ci.Rooms)
                .HasForeignKey(ci => ci.ClubId)
                .IsRequired();
        }
    }
}
