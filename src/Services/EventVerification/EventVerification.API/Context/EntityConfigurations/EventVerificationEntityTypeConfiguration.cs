using System;
using EMS.EventVerification_Services.API.Context.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.EventVerification_Services.API.Context.EntityConfigurations
{
    class EventVerificationEntityTypeConfiguration
        : IEntityTypeConfiguration<EventVerification>
    {
        public void Configure(EntityTypeBuilder<EventVerification> builder)
        {
            builder.ToTable("EventVerification");

            builder.HasKey(ci => ci.EventVerificationId);

            builder.Property(ci => ci.EventId)
                .IsRequired();

            builder.Property(ci => ci.UserId)
                .IsRequired();

            builder.Property(ci => ci.EventVerificationId)
                .IsRequired();
        }
    }
}
