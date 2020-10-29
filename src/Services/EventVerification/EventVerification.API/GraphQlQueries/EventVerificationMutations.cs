using System;
using System.Threading.Tasks;
using AutoMapper;
using EMS.Events;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.EntityFrameworkCore;
using EMS.EventVerification_Services.API.Context;
using EMS.EventVerification_Services.API.Controllers.Request;
using EMS.TemplateWebHost.Customization.EventService;
using EMS.EventVerification_Services.API.Context.Model;
using EMS.TemplateWebHost.Customization.StartUp;
using Microsoft.AspNetCore.Authorization;

namespace EMS.EventVerification_Services.API.GraphQlQueries
{
    public class EventVerificationMutations : BaseMutations
    {
        private readonly EventVerificationContext _context;
        private readonly IMapper _mapper;
        private readonly IEventService _eventService;

        public EventVerificationMutations(EventVerificationContext context, IEventService template1EventService, IMapper mapper, IAuthorizationService authorizationService) : base(authorizationService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); ;
            _eventService = template1EventService ?? throw new ArgumentNullException(nameof(template1EventService));
            _mapper = mapper;
        }

        public async Task<EventVerification> VerifyCodeAsync(VerifyCodeRequest request)
        {
            var codeInt = int.Parse(request.Code, System.Globalization.NumberStyles.HexNumber);
            var item = await _context.EventVerifications
                .SingleOrDefaultAsync(ci => ci.EventId == request.EventId
                                            && ci.EventVerificationId == codeInt);

            if (item == null)
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetMessage("Invalid code or eventId")
                        .SetCode("ID_UNKNOWN")
                        .Build());
            }

            if (item.Status != PresenceStatusEnum.SignedUp)
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetMessage("EventVerificationId have already been used")
                        .SetCode("ID_UNKNOWN")
                        .Build());
            }

            item.Status = PresenceStatusEnum.Attend;
            _context.EventVerifications.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }
    }
}
