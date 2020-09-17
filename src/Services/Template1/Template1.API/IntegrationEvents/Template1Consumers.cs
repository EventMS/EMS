using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Events;
using MassTransit;
using Serilog;

namespace Template1.API.IntegrationEvents
{
    public class Template1CreatedIntegrationEventConsumer :
            IConsumer<Template1CreatedIntegrationEvent>
        {
            public async Task Consume(ConsumeContext<Template1CreatedIntegrationEvent> context)
            {
                Log.Information("Template1Value: {Value}", context.Message.Name);
                Log.Information("Template1Value: {Value}", context.Message.CreationDate);
                Log.Information("Template1Value: {Value}", context.Message.Id);
            }
        }
    }
