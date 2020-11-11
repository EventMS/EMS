using System;
using System.Linq;
using System.Threading.Tasks;
using EMS.Events;
using EMS.EventVerification_Services.API.Context;
using EMS.EventVerification_Services.API.Context.Model;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace EMS.EventVerification_Services.API.Events
{
    public class SignUpEventSuccessEventConsumer :
            IConsumer<SignUpEventSuccessEvent>
    {
        private readonly EventVerificationContext _context;

        public SignUpEventSuccessEventConsumer(EventVerificationContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<SignUpEventSuccessEvent> context)
        {
            var ev = await _context.EventVerifications
                .SingleOrDefaultAsync(ev => ev.UserId == context.Message.UserId && ev.EventId == context.Message.EventId);
            if (ev == null)
            {
                ev = new EventVerification()
                {
                    EventId = context.Message.EventId,
                    UserId = context.Message.UserId,
                };
                await _context.EventVerifications.AddAsync(ev);
                await _context.SaveChangesAsync();
            }
        }
    }
}
