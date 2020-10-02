using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Subscription.API.Context.EntityConfigurations;
using TemplateWebHost.Customization.Context;

namespace Subscription.API.Context
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
                .UseSqlServer("Server=.;Initial Catalog=Microsoft.eShopOnContainers.Services.SubscriptionDb;Integrated Security=true");

            return new SubscriptionContext(optionsBuilder.Options);
        }
    }
}
