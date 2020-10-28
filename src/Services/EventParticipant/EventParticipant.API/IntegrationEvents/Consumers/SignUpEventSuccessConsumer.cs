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
    public class SignUpEventSuccessEventConsumer :
        IConsumer<SignUpEventSuccessEvent>
    {
        private readonly EventParticipantContext _context;

        public SignUpEventSuccessEventConsumer(EventParticipantContext context)
        {
            _context = context;
        }



        public async Task Consume(ConsumeContext<SignUpEventSuccessEvent> context)
        {
            if (await _context.Events.FindAsync(context.Message.EventId) == null)
            {
                Log.Information("Tried to sign up to a event that did not exist");
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