using HotChocolate;
using Serilog;

namespace TemplateWebHost.Customization.Filters
{
    public class GraphQLErrorFilter : IErrorFilter
    {
        public IError OnError(IError error)
        {
            Log.Information(error.Exception.Message);

            return error.WithMessage(error.Exception.Message + error.Exception.InnerException?.Message)
                .WithCode(error.Exception.GetType().ToString())
                .WithException(error.Exception.InnerException);
        }
    }
}