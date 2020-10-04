using AutoMapper;
using EMS.Club_Service.API.Controllers.Request;
using EMS.Events;

namespace EMS.Club_Service.API.Mapper
{
    public class ClubProfile : Profile
    {
        public ClubProfile()
        {
            CreateMap<CreateClubRequest,Context.Model.Club>();
            CreateMap<UpdateClubRequest, Context.Model.Club>();
            CreateMap<Context.Model.Club, ClubCreatedEvent>();
            CreateMap<Context.Model.Club, ClubUpdatedEvent>();
            CreateMap<Context.Model.Club, ClubDeletedEvent>();
        }
    }
}
