using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Events;
using MassTransit;
using Serilog;

namespace Club.API.IntegrationEvents
{
    public class UserCreatedIntegrationEventClubConsumer :
            IConsumer<UserCreatedIntegrationEvent>
        {
            public async Task Consume(ConsumeContext<UserCreatedIntegrationEvent> context)
            {
                Log.Information("User created event name: {Value}", context.Message.Name);
                Log.Information("User Id : {Value}", context.Message.Id);
            }
        }
    }
