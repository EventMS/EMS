using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using EMS.ClubMember_Services.API.Context.EntityConfigurations;
using EMS.TemplateWebHost.Customization.Context;

namespace EMS.ClubMember_Services.API.Context
{
    using Model;
    public class ClubMemberContext : BaseContext
    {
        public ClubMemberContext(DbContextOptions<ClubMemberContext> options) : base(options)
        {
        }
        public DbSet<ClubMember> ClubMembers { get; set; }
        public DbSet<ClubSubscription> ClubSubscriptions { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ClubMemberEntityTypeConfiguration());
            builder.ApplyConfiguration(new ClubSubscriptionEntityTypeConfiguration());
        }
    }


    public class ClubMemberContextDesignFactory : IDesignTimeDbContextFactory<ClubMemberContext>
    {
        public ClubMemberContext CreateDbContext(string[] args)
        {
            var optionsBuilder =  new DbContextOptionsBuilder<ClubMemberContext>()
                .UseSqlServer("Server=.;Initial Catalog=Services.ClubMemberDb;Integrated Security=true");

            return new ClubMemberContext(optionsBuilder.Options);
        }
    }
}
