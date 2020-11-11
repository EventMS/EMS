using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using EMS.EventVerification_Services.API.Context.EntityConfigurations;
using EMS.TemplateWebHost.Customization.Context;

namespace EMS.EventVerification_Services.API.Context
{
    using Model;
    public class EventVerificationContext : BaseContext
    {
        public EventVerificationContext(DbContextOptions<EventVerificationContext> options) : base(options)
        {
        }
        public DbSet<EventVerification> EventVerifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new EventVerificationEntityTypeConfiguration());
        }
    }


    public class EventVerificationContextDesignFactory : IDesignTimeDbContextFactory<EventVerificationContext>
    {
        public EventVerificationContext CreateDbContext(string[] args)
        {
            var optionsBuilder =  new DbContextOptionsBuilder<EventVerificationContext>()
                .UseSqlServer("Server=.;Initial Catalog=Services.EventVerificationDb;Integrated Security=true");

            return new EventVerificationContext(optionsBuilder.Options);
        }
    }
}
