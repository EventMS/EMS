
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using EMS.Payment_Services.API.Context.EntityConfigurations;
using EMS.TemplateWebHost.Customization.Context;

namespace EMS.Payment_Services.API.Context
{
    using Model;
    public class PaymentContext : BaseContext
    {
        public PaymentContext(DbContextOptions<PaymentContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<ClubSubscription> ClubSubscriptions { get; set; }
        public DbSet<EventPrice> EventPrices { get; set; }

        public DbSet<Event> Events { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserEntityTypeConfiguration());
            builder.ApplyConfiguration(new ClubSubscriptionEntityTypeConfiguration());
            builder.ApplyConfiguration(new EventEntityTypeConfiguration());
            builder.ApplyConfiguration(new EventPriceEntityTypeConfiguration());
        }
    }


    public class PaymentContextDesignFactory : IDesignTimeDbContextFactory<PaymentContext>
    {
        public PaymentContext CreateDbContext(string[] args)
        {
            var optionsBuilder =  new DbContextOptionsBuilder<PaymentContext>()
                .UseSqlServer("Server=.;Initial Catalog=Services.PaymentDb;Integrated Security=true");

            return new PaymentContext(optionsBuilder.Options);
        }
    }
}
