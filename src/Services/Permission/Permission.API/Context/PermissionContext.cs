using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using EMS.Permission_Services.API.Context.EntityConfigurations;
using EMS.TemplateWebHost.Customization.Context;

namespace EMS.Permission_Services.API.Context
{
    using Model;
    public class PermissionContext : BaseContext
    {
        public PermissionContext(DbContextOptions<PermissionContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Club> Clubs { get; set; }

        public DbSet<Role> Roles { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserPermissionEntityTypeConfiguration());
            builder.ApplyConfiguration(new ClubEntityTypeConfiguration());
            builder.ApplyConfiguration(new RoleEntityTypeConfiguration());
        }
    }


    public class PermissionContextDesignFactory : IDesignTimeDbContextFactory<PermissionContext>
    {
        public PermissionContext CreateDbContext(string[] args)
        {
            var optionsBuilder =  new DbContextOptionsBuilder<PermissionContext>()
                .UseSqlServer("Server=.;Initial Catalog=Services.PermissionDb;Integrated Security=true");

            return new PermissionContext(optionsBuilder.Options);
        }
    }
}
