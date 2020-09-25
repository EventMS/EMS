using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Events;
using MassTransit;
using Serilog;

namespace Template1.API.Events
{
    public class Template1CreatedEventConsumer :
            IConsumer<Template1CreatedEvent>
        {
            public async Task Consume(ConsumeContext<Template1CreatedEvent> context)
            {
                Log.Information("Template1Value: {Value}", context.Message.Name);
                Log.Information("Template1Value: {Value}", context.Message.CreationDate);
                Log.Information("Template1Value: {Value}", context.Message.Id);
            }
        }
    }
