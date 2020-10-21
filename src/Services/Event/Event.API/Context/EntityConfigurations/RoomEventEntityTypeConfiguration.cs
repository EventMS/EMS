using EMS.Event_Services.API.Context.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Event_Services.API.Context.EntityConfigurations
{
    class RoomEventEntityTypeConfiguration
        : IEntityTypeConfiguration<RoomEvent>
    {
        public void Configure(EntityTypeBuilder<RoomEvent> builder)
        {
            builder.ToTable("RoomEvent");

            builder.HasKey(ci => new { ci.EventId, ci.RoomId });
            builder.HasOne(bc => bc.Event)
                .WithMany(b => b.Locations)
                .HasForeignKey(bc => bc.EventId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(bc => bc.Room)
                .WithMany(c => c.Locations)
                .HasForeignKey(bc => bc.RoomId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}