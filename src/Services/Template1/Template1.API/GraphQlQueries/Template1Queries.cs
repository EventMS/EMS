using System.Linq;
using EMS.Template1_Services.API.Context;
using EMS.Template1_Services.API.Context.Model;

namespace EMS.Template1_Services.API.GraphQlQueries
{
    public class Template1Queries
    {
        private readonly Template1Context _context;
        public Template1Queries(Template1Context context)
        {
            _context = context;
        }

        public IQueryable<Template1> Template1s => _context.Template1s.AsQueryable();
    }
}