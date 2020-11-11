using EMS.Event_Services.API.Context.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.Event_Services.API.Context.EntityConfigurations
{
    class InstructorEntityTypeConfiguration
        : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            builder.ToTable("Instructor");

            builder.HasKey(ci => ci.InstructorId);

            builder.HasOne<Club>()
                .WithMany()
                .HasForeignKey(ci => ci.ClubId)
                .IsRequired();
        }
    }
}