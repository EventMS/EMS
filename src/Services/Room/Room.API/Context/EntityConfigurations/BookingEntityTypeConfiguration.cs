using EMS.Room_Services.API.Context.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Room_Services.API.Context.EntityConfigurations
{
    class BookingEntityTypeConfiguration
        : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.ToTable("Booking");

            builder.HasKey(ci => new
            {
                ci.EventId, ci.RoomId
            });

            builder.Property(ci => ci.StartTime)
                .IsRequired();

            builder.Property(ci => ci.EndTime)
                .IsRequired();

            builder.HasOne<Room>()
                .WithMany(ci => ci.Bookings)
                .HasForeignKey(ci => ci.RoomId)
                .IsRequired();
        }
    }
}