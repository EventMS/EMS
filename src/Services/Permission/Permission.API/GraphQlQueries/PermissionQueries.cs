using System.Linq;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Permission.API.Context;
using Permission.API.Context.Model;

namespace Permission.API.GraphQlQueries
{
    public class PermissionQueries
    {
        private readonly PermissionContext _context;
        public PermissionQueries(PermissionContext context)
        {
            _context = context;
        }

        [UsePaging]
        [UseFiltering]
        public IQueryable<UserPermission> Permissions => _context.UserPermissions.AsQueryable();
    }
}
