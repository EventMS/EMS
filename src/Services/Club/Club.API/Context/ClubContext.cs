using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Club.API.Context.EntityConfigurations;
using TemplateWebHost.Customization.Context;

namespace Club.API.Context
{
    using Model;
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
                .UseSqlServer("Server=.;Initial Catalog=Microsoft.eShopOnContainers.Services.ClubDb;Integrated Security=true");

            return new ClubContext(optionsBuilder.Options);
        }
    }
}
