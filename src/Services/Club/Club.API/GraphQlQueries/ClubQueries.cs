using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Club.API.Context;
using Microsoft.EntityFrameworkCore;

namespace Club.API.GraphQlQueries
{
    public class ClubQueries
    {
        private readonly ClubContext _context;
        public ClubQueries(ClubContext context)
        {
            _context = context;
        }

        public IQueryable<Context.Model.Club> Clubs => _context.Clubs.AsQueryable();
    }
}
