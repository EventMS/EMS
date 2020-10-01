using HotChocolate;
using Serilog;

namespace TemplateWebHost.Customization.Filters
{
    public class GraphQlErrorFilter : IErrorFilter
    {
        public IError OnError(IError error)
        {
            if (error.Message.Equals("Unexpected Execution Error"))
            {
                Log.Information(error.Exception.Message);
                Log.Information(error.Exception.InnerException?.Message);
                return error.WithCode(error.Exception.GetType().ToString())
                    .WithMessage(error.Exception.Message + error.Exception.InnerException?.Message);
            }
            return error;
        }
    }
}