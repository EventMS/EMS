using AutoMapper;
using EMS.Events;
using EMS.Room_Services.API.Context.Model;
using EMS.Room_Services.API.Controllers.Request;

namespace EMS.Room_Services.API.Mapper
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<CreateRoomRequest, Room>();
            CreateMap<UpdateRoomRequest, Room>();
            CreateMap<Room, RoomCreatedEvent>();
            CreateMap<Room, RoomUpdatedEvent>();
            CreateMap<Room, RoomDeletedEvent>();
        }
    }
}
