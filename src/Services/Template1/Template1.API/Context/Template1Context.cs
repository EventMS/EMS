using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using EMS.Template1_Services.API.Context.EntityConfigurations;
using EMS.TemplateWebHost.Customization.Context;

namespace EMS.Template1_Services.API.Context
{
    using Model;
    public class Template1Context : BaseContext
    {
        public Template1Context(DbContextOptions<Template1Context> options) : base(options)
        {
        }
        public DbSet<Template1> Template1s { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new Template1EntityTypeConfiguration());
        }
    }


    public class Template1ContextDesignFactory : IDesignTimeDbContextFactory<Template1Context>
    {
        public Template1Context CreateDbContext(string[] args)
        {
            var optionsBuilder =  new DbContextOptionsBuilder<Template1Context>()
                .UseSqlServer("Server=.;Initial Catalog=Services.Template1Db;Integrated Security=true");

            return new Template1Context(optionsBuilder.Options);
        }
    }
}
