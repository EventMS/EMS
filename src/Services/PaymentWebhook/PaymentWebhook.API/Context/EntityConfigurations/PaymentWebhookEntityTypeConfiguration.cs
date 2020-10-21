using EMS.PaymentWebhook_Services.API.Context.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EMS.PaymentWebhook_Services.API.Context.EntityConfigurations
{
    class PaymentWebhookEntityTypeConfiguration
        : IEntityTypeConfiguration<PaymentWebhook>
    {
        public void Configure(EntityTypeBuilder<PaymentWebhook> builder)
        {
            builder.ToTable("PaymentWebhook");

            builder.HasKey(ci => ci.PaymentWebhookId);

            builder.Property(ci => ci.PaymentWebhookId)
                .IsRequired();

            builder.Property(ci => ci.Name)
                .IsRequired();
        }
    }
}
