using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using EMS.Subscription_Services.API.Context.EntityConfigurations;
using EMS.TemplateWebHost.Customization.Context;

namespace EMS.Subscription_Services.API.Context
{
    public class SubscriptionContext : BaseContext
    {
        public SubscriptionContext(DbContextOptions<SubscriptionContext> options) : base(options)
        {
        }
        public DbSet<ClubSubscription> ClubSubscriptions { get; set; }
        public DbSet<Club> Clubs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ClubSubscriptionEntityTypeConfiguration());
            builder.ApplyConfiguration(new ClubEntityTypeConfiguration());
        }
    }


    public class SubscriptionContextDesignFactory : IDesignTimeDbContextFactory<SubscriptionContext>
    {
        public SubscriptionContext CreateDbContext(string[] args)
        {
            var optionsBuilder =  new DbContextOptionsBuilder<SubscriptionContext>()
                .UseSqlServer("Server=.;Initial Catalog=Services.SubscriptionDb;Integrated Security=true");

            return new SubscriptionContext(optionsBuilder.Options);
        }
    }
}
