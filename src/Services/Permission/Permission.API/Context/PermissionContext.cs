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
    public class PermissionContext : Base1Context<PermissionContext>
    {
        public PermissionContext(DbContextOptions<PermissionContext> options) : base(options)
        {
        }
        public DbSet<Permission> Permissions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new PermissionEntityTypeConfiguration());
        }

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var changedEntities = ChangeTracker
                .Entries()
                .Where(_ => _.State == EntityState.Added ||
                            _.State == EntityState.Modified);

            var errors = new List<ValidationResult>(); // all errors are here
            foreach (var e in changedEntities)
            {
                var vc = new ValidationContext(e.Entity, null, null);
                Validator.ValidateObject(
                    e.Entity, vc, true);
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
    }


    public class PermissionContextDesignFactory : IDesignTimeDbContextFactory<PermissionContext>
    {
        public PermissionContext CreateDbContext(string[] args)
        {
            var optionsBuilder =  new DbContextOptionsBuilder<PermissionContext>()
                .UseSqlServer("Server=.;Initial Catalog=Microsoft.eShopOnContainers.Services.PermissionDb;Integrated Security=true");

            return new PermissionContext(optionsBuilder.Options);
        }
    }
}
