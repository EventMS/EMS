using AutoMapper;
using EMS.Events;
using EMS.Subscription_Services.API.Context;
using EMS.Subscription_Services.API.GraphQlQueries.Request;

namespace EMS.Subscription_Services.API.Mapper
{
    public class SubscriptionProfile : Profile
    {
        public SubscriptionProfile()
        {
            CreateMap<CreateClubSubscriptionRequest, ClubSubscription>();
            CreateMap<UpdateClubSubscriptionRequest, ClubSubscription>();
            CreateMap<ClubSubscription, ClubSubscriptionCreatedEvent>();
            CreateMap<ClubSubscription, ClubSubscriptionUpdatedEvent>();
            CreateMap<ClubCreatedEvent, Club>();
        }
    }
}
