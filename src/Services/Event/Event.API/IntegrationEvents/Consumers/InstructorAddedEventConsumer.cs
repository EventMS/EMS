using System.Threading.Tasks;
using EMS.Event_Services.API.Context;
using EMS.Event_Services.API.Context.Model;
using EMS.Events;
using MassTransit;

namespace EMS.Event_Services.API.Events
{
    public class InstructorAddedEventConsumer :
        IConsumer<InstructorAddedEvent>
    {
        private readonly EventContext _context;

        public InstructorAddedEventConsumer(EventContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<InstructorAddedEvent> context)
        {
            var instructor = _context.Instructors.Find(context.Message.UserId);
            if (instructor == null)
            {
                _context.Instructors.Add(new Instructor()
                {
                    ClubId = context.Message.ClubId,
                    InstructorId = context.Message.UserId
                });
                await _context.SaveChangesAsync();
            }
        }
    }
}