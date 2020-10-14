using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EMS.EventParticipant_Services.API.Context;
using EMS.EventParticipant_Services.API.Context.Model;
using EMS.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Serilog;
using TemplateWebHost.Customization.BasicConsumers;

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
            if (await _context.Events.FindAsync(context.Message.EventId) == null)
            {
                return;
            }
            var ep = await _context.EventParticipants.FirstOrDefaultAsync(ep =>
                ep.EventId == context.Message.EventId && ep.UserId == context.Message.UserId);

            if (ep == null)
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