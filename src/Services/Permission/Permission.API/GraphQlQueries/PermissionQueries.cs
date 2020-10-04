using System.Linq;
using EMS.Permission_Services.API.Context;
using EMS.Permission_Services.API.Context.Model;

namespace EMS.Permission_Services.API.GraphQlQueries
{
    public class PermissionQueries
    {
        private readonly PermissionContext _context;
        public PermissionQueries(PermissionContext context)
        {
            _context = context;
        }

        public IQueryable<UserPermission> Permissions => _context.UserPermissions.AsQueryable();
    }
}
