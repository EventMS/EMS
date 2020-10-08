using EMS.Event_Services.API.Context.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Event_Services.API.Context.EntityConfigurations
{
    class RoomEntityTypeConfiguration
        : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.ToTable("Room");

            builder.HasKey(ci => ci.RoomId);

            builder.HasOne<Club>()
                .WithMany()
                .HasForeignKey(ci => ci.ClubId);
        }
    }
}