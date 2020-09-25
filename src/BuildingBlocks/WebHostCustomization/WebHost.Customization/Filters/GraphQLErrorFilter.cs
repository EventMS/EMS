using HotChocolate;
using Serilog;

namespace TemplateWebHost.Customization.Filters
{
    public class GraphQLErrorFilter : IErrorFilter
    {
        public IError OnError(IError error)
        {
            Log.Information(error.Exception.Message);
            Log.Information(error.Message);
            Log.Information(error.Exception.InnerException?.Message);
            return error;
        }
    }
}