using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Events;
using MassTransit;
using Serilog;

namespace Permission.API.IntegrationEvents
{
    public class PermissionCreatedIntegrationEventConsumer :
            IConsumer<PermissionCreatedIntegrationEvent>
        {
            public async Task Consume(ConsumeContext<PermissionCreatedIntegrationEvent> context)
            {
                Log.Information("PermissionValue: {Value}", context.Message.Name);
                Log.Information("PermissionValue: {Value}", context.Message.CreationDate);
                Log.Information("PermissionValue: {Value}", context.Message.Id);
            }
        }
    }
