using EMS.Event_Services.API.Context.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Event_Services.API.Context.EntityConfigurations
{
    class InstructorForEventsEntityTypeConfiguration
        : IEntityTypeConfiguration<InstructorForEvent>
    {
        public void Configure(EntityTypeBuilder<InstructorForEvent> builder)
        {
            builder.ToTable("InstructorForEvent");

            builder.HasKey(ci => new { ci.EventId, ci.InstructorId });

            builder.HasOne(bc => bc.Event)
                .WithMany(b => b.InstructorForEvents)
                .HasForeignKey(bc => bc.EventId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(bc => bc.Instructor)
                .WithMany(c => c.InstructorForEvents)
                .HasForeignKey(bc => bc.InstructorId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}