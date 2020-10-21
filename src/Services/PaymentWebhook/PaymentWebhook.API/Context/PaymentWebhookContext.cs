using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using EMS.PaymentWebhook_Services.API.Context.EntityConfigurations;
using EMS.TemplateWebHost.Customization.Context;

namespace EMS.PaymentWebhook_Services.API.Context
{
    using Model;
    public class PaymentWebhookContext : BaseContext
    {
        public PaymentWebhookContext(DbContextOptions<PaymentWebhookContext> options) : base(options)
        {
        }
        public DbSet<PaymentWebhook> PaymentWebhooks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new PaymentWebhookEntityTypeConfiguration());
        }
    }


    public class PaymentWebhookContextDesignFactory : IDesignTimeDbContextFactory<PaymentWebhookContext>
    {
        public PaymentWebhookContext CreateDbContext(string[] args)
        {
            var optionsBuilder =  new DbContextOptionsBuilder<PaymentWebhookContext>()
                .UseSqlServer("Server=.;Initial Catalog=Services.PaymentWebhookDb;Integrated Security=true");

            return new PaymentWebhookContext(optionsBuilder.Options);
        }
    }
}
