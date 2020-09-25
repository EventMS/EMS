using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Events;
using MassTransit;
using Serilog;

namespace Club.API.Events
{
    public class UserCreatedEventClubConsumer :
            IConsumer<UserCreatedEvent>
        {
            public async Task Consume(ConsumeContext<UserCreatedEvent> context)
            {
                Log.Information("User created event name: {Value}", context.Message.Name);
                Log.Information("User Id : {Value}", context.Message.Id);
            }
        }
    }
