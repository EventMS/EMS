using System.Linq;
using EMS.Identity_Services.API.Context.Models;
using EMS.Identity_Services.API.Data;

namespace EMS.Identity_Services.API.GraphQlQueries
{
    public class IdentityQueries
    {
        private readonly ApplicationDbContext _context;
        public IdentityQueries(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<ApplicationUser> Users => _context.Users.AsQueryable();
    }
}