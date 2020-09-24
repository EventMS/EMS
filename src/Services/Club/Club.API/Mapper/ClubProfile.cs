using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Club.API.Controllers.Request;
using EMS.Events;

namespace Club.API.Mapper
{
    public class ClubProfile : Profile
    {
        public ClubProfile()
        {
            CreateMap<CreateClubRequest,Context.Model.Club>();
            CreateMap<UpdateClubRequest, Context.Model.Club>();
            CreateMap<Context.Model.Club, ClubCreatedIntegrationEvent>();
            CreateMap<Context.Model.Club, ClubUpdatedIntegrationEvent>();
            CreateMap<Context.Model.Club, ClubDeletedIntegrationEvent>();
        }
    }
}
