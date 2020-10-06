using AutoMapper;
using EMS.Events;
using EMS.ClubMember_Services.API.Context.Model;
using EMS.ClubMember_Services.API.Controllers.Request;

namespace EMS.ClubMember_Services.API.Mapper
{
    public class ClubMemberProfile : Profile
    {
        public ClubMemberProfile()
        {
            CreateMap<CreateClubMemberRequest, ClubMember>();
            CreateMap<UpdateClubMemberRequest, ClubMember>();
            CreateMap<ClubMember, ClubMemberCreatedEvent>();
            CreateMap<ClubMember, ClubMemberUpdatedEvent>();
            CreateMap<ClubMember, ClubMemberDeletedEvent>();
        }
    }
}
