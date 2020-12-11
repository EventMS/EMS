using System;
using System.IO;
using System.Text;
using HotChocolate.Execution;
using HotChocolate.Execution.Instrumentation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DiagnosticAdapter;
using Microsoft.Extensions.Logging;

namespace EMS.TemplateWebHost.Customization.StartUp
{
    /// <summary>
    /// Simple class that implements basic logging for GraphQL.
    /// </summary>
    public class DiagnosticObserver
        : IDiagnosticObserver
    {
        private readonly ILogger _logger;

        public DiagnosticObserver(ILogger<DiagnosticObserver> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [DiagnosticName("HotChocolate.Execution.Query")]
        public void OnQuery(IQueryContext context)
        {
            // This method is used as marker to enable begin and end events
            // in the case that you want to explicitly track the start and the
            // end of this event.
        }

        [DiagnosticName("HotChocolate.Execution.Query.Start")]
        public void BeginQueryExecute(IQueryContext context)
        {
            _logger.LogInformation("Query start----" + context.Request.Query.ToString());
        }

        [DiagnosticName("HotChocolate.Execution.Query.Stop")]
        public void EndQueryExecute(IQueryContext context)
        {
            if (context.Result is IReadOnlyQueryResult result)
            {
                using (var stream = new MemoryStream())
                {
                    var resultSerializer = new JsonQueryResultSerializer();
                    resultSerializer.SerializeAsync(
                        result, stream).AsTask().Wait();
                    _logger.LogInformation(
                        "Query Result ----" +Encoding.UTF8.GetString(stream.ToArray()));
                }
            }
        }
    }
}