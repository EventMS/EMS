using AutoMapper;
using EMS.Events;
using EMS.EventVerification_Services.API.Context.Model;
using EMS.EventVerification_Services.API.Controllers.Request;

namespace EMS.EventVerification_Services.API.Mapper
{
    public class EventVerificationProfile : Profile
    {
        public EventVerificationProfile()
        {
            CreateMap<VerifyCodeRequest, EventVerification>();
        }
    }
}
