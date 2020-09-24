using AutoMapper;
using EMS.Events;
using Template1.API.Controllers.Request;

namespace Template1.API.Mapper
{
    public class Template1Profile : Profile
    {
        public Template1Profile()
        {
            CreateMap<CreateTemplate1Request, Template1.API.Context.Model.Template1>();
            CreateMap<UpdateTemplate1Request, Template1.API.Context.Model.Template1>();
            CreateMap<Template1.API.Context.Model.Template1, Template1CreatedIntegrationEvent>();
            CreateMap<Template1.API.Context.Model.Template1, Template1UpdatedIntegrationEvent>();
            CreateMap<Template1.API.Context.Model.Template1, Template1DeletedIntegrationEvent>();
        }
    }
}
