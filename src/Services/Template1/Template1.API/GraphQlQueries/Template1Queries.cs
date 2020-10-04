using System.Linq;
using EMS.Template1_Services.API.Context;

namespace EMS.Template1_Services.API.GraphQlQueries
{
    public class Template1Queries
    {
        private readonly Template1Context _context;
        public Template1Queries(Template1Context context)
        {
            _context = context;
        }

        public IQueryable<Context.Model.Template1> Template1s => _context.Template1s.AsQueryable();
    }
}