using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Permission.API.Context.EntityConfigurations;
using TemplateWebHost.Customization.Context;

namespace Permission.API.Context
{
    using Model;
    public class PermissionContext : BaseContext
    {
        public PermissionContext(DbContextOptions<PermissionContext> options) : base(options)
        {
        }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<ClubAdministratorPermission> ClubAdministratorPermissions { get; set; }

        public DbSet<UserAdministratorPermission> UserAdministratorPermission { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserPermissionEntityTypeConfiguration());
            builder.ApplyConfiguration(new ClubAdministratorPermissionEntityTypeConfiguration());
            builder.ApplyConfiguration(new UserAdministratorPermissionEntityTypeConfiguration());
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
