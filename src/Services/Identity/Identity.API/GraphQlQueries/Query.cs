using System.Linq;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Identity.API.Data;
using Microsoft.eShopOnContainers.Services.Identity.API.Models;

namespace Identity.API.GraphQlQueries
{
    public class Query
    {
        private readonly ApplicationDbContext _context;
        public Query(ApplicationDbContext context)
        {
            _context = context;
        }

        [UsePaging]
        [UseFiltering]
        public IQueryable<ApplicationUser> Users => _context.Users.AsQueryable();
    }
}