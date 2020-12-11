using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using EMS.Club_Service.API.Context.EntityConfigurations;
using EMS.TemplateWebHost.Customization.Context;

namespace EMS.Club_Service.API.Context
{
    using Model;
    /// <summary>
    /// Club context that inherits from Base Context
    /// </summary>
    public class ClubContext : BaseContext
    {
        public ClubContext(DbContextOptions<ClubContext> options) : base(options)
        {
        }
        public DbSet<Club> Clubs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ClubEntityTypeConfiguration());
        }
    }


    public class ClubContextDesignFactory : IDesignTimeDbContextFactory<ClubContext>
    {
        public ClubContext CreateDbContext(string[] args)
        {
            var optionsBuilder =  new DbContextOptionsBuilder<ClubContext>()
                .UseSqlServer("Server=.;Initial Catalog=Services.ClubDb;Integrated Security=true");

            return new ClubContext(optionsBuilder.Options);
        }
    }
}
