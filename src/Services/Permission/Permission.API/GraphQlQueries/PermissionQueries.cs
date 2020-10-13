using System.Linq;
using EMS.Permission_Services.API.Context;
using EMS.Permission_Services.API.Context.Model;
using Microsoft.EntityFrameworkCore;

namespace EMS.Permission_Services.API.GraphQlQueries
{
    public class PermissionQueries
    {
        private readonly PermissionContext _context;
        public PermissionQueries(PermissionContext context)
        {
            _context = context;
        }

        public IQueryable<User> Permissions => _context.Users.Include(u => u.Roles).AsQueryable();
    }
}
