using HotChocolate;
using HotChocolate.Resolvers;
using Serilog;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.TemplateWebHost.Customization.Filters
{
    /// <summary>
    /// Simple graphql error filtering. Is used to decorate error objects from exceptions. 
    /// </summary>
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
    /// <summary>
    /// Input validation middleware that ensures Attribute input validation as "Empty", "FutureDate" are applied.
    /// </summary>
    public class ValidateInputMiddleware
    {
        private readonly FieldDelegate _next;

        public ValidateInputMiddleware(FieldDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(IMiddlewareContext context)
        {
            if (context.FieldSelection.Arguments.Count == 0)
            {
                await _next(context);
                return;
            }

            var errors = context.FieldSelection.Arguments
                .Select(a => context.Argument<object>(a.Name.Value))
                .SelectMany(ValidateObject);

            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    
                    context.ReportError(ErrorBuilder.New()
                        .SetCode("error.validation")
                        .SetMessage(error.ErrorMessage)
                        .SetExtension("memberNames", error.MemberNames)
                        .AddLocation(context.FieldSelection.Location.Line, context.FieldSelection.Location.Column)
                        .SetPath(context.Path)
                        .Build());
                }

                context.Result = null;

            }
            else
            {
                await _next(context);
            }

            IEnumerable<ValidationResult> ValidateObject(object argument)
            {
                var results = new List<ValidationResult>();

                Validator.TryValidateObject(argument, new ValidationContext(argument), results, validateAllProperties: true);

                return results;
            }
        }
    }

}