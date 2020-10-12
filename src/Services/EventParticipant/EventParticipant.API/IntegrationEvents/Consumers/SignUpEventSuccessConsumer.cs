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
        BasicDuplicateConsumer<EventParticipantContext, EventParticipant, SignUpEventSuccess>
    {
        public SignUpEventSuccessConsumer(EventParticipantContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public override async Task<EventParticipant> FindEntity(EventParticipant entity, SignUpEventSuccess e)
        {
            return await _context.EventParticipants.FirstOrDefaultAsync(ep =>
                ep.EventId == e.EventId && ep.UserId == e.UserId);
        }
    }
}