using HotChocolate;
using Serilog;

namespace TemplateWebHost.Customization.Filters
{
    public class GraphQLErrorFilter : IErrorFilter
    {
        public IError OnError(IError error)
        {
            return error;
        }
    }
}