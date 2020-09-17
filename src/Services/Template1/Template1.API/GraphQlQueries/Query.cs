using System.Linq;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Template1.API.Context;

namespace Template1.API.GraphQlQueries
{
    public class Query
    {
        private readonly Template1Context _context;
        public Query(Template1Context context)
        {
            _context = context;
        }

        [UsePaging]
        [UseFiltering]
        public IQueryable<Context.Model.Template1> Template1s => _context.Template1s.AsQueryable();
    }
}