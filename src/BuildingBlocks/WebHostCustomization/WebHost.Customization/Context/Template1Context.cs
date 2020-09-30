using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TemplateWebHost.Customization.Context
{
    public abstract class BaseContext : DbContext
    {
        protected BaseContext(DbContextOptions options) : base(options)
        {
        }

        public override int SaveChanges()
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
            return base.SaveChanges();
        }



        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
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
}
