using AutoMapper;
using EMS.Events;
using EMS.Template1_Services.API.Context.Model;
using EMS.Template1_Services.API.Controllers.Request;

namespace EMS.Template1_Services.API.Mapper
{
    public class Template1Profile : Profile
    {
        public Template1Profile()
        {
            CreateMap<CreateTemplate1Request, Template1>();
            CreateMap<UpdateTemplate1Request, Template1>();
            CreateMap<Template1, Template1CreatedEvent>();
            CreateMap<Template1, Template1UpdatedEvent>();
            CreateMap<Template1, Template1DeletedEvent>();
        }
    }
}
