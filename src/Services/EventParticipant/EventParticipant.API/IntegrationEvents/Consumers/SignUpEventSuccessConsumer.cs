using System.Linq;
using System.Threading.Tasks;
using EMS.EventParticipant_Services.API.Context;
using EMS.EventParticipant_Services.API.Context.Model;
using EMS.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace EMS.EventParticipant_Services.API.Events
{
    public class SignUpEventSuccessConsumer :
        IConsumer<SignUpEventSuccess>
    {
        private readonly EventParticipantContext _context;

        public SignUpEventSuccessConsumer(EventParticipantContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<SignUpEventSuccess> context)
        {
            var item = await _context.EventParticipants.FirstOrDefaultAsync(ep =>
                ep.EventId == context.Message.EventId && ep.UserId == context.Message.UserId);
            if (item == null)
            {
                await _context.EventParticipants.AddAsync(new EventParticipant()
                {
                    EventId = context.Message.EventId,
                    UserId = context.Message.UserId
                });
                await _context.SaveChangesAsync();
            }
        }
    }
}