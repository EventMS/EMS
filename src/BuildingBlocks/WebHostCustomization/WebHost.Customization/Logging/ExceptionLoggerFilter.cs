using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GreenPipes;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Context;
using MassTransit.PipeConfigurators;
using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Serilog.ILogger;

namespace EMS.TemplateWebHost.Customization.Masstransit
{
    public static class MessageFilterConfigurationExtensions
    {
        public static void UseMessageFilter(this IConsumePipeConfigurator configurator)
        {
            if (configurator == null)
                throw new ArgumentNullException(nameof(configurator));

            var observer = new MessageFilterConfigurationObserver(configurator);
        }
    }

    public class MessageFilterConfigurationObserver :
        ConfigurationObserver,
        IMessageConfigurationObserver
    {
        public MessageFilterConfigurationObserver(IConsumePipeConfigurator receiveEndpointConfigurator)
            : base(receiveEndpointConfigurator)
        {
            Connect(this);
        }

        public void MessageConfigured<TMessage>(IConsumePipeConfigurator configurator)
            where TMessage : class
        {
            var specification = new MessageFilterPipeSpecification<TMessage>();

            configurator.AddPipeSpecification(specification);
        }
    }

    public class MessageFilterPipeSpecification<T> :
        IPipeSpecification<ConsumeContext<T>>
        where T : class
    {
        public void Apply(IPipeBuilder<ConsumeContext<T>> builder)
        {
            var filter = new MessageFilter<T>();

            builder.AddFilter(filter);
        }

        public IEnumerable<ValidationResult> Validate()
        {
            yield break;
        }
    }

    public class MessageFilter<T> :
        IFilter<ConsumeContext<T>>
        where T : class
    {

        public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
        {
            try
            {
                Log.Information("----- Receiving event: ({@Event})", context.Message);
                await next.Send(context);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"An exception occurred: {ex.Message}");

                // propagate the exception up the call stack
                throw;
            }
            
        }

        public void Probe(ProbeContext context)
        {
            var scope = context.CreateFilterScope("messageFilter");
        }
    }
}
