using AutoMapper;
using EMS.Events;
using EMS.Subscription_Services.API.GraphQlQueries.Request;

namespace EMS.Subscription_Services.API.Mapper
{
    public class SubscriptionProfile : Profile
    {
        public SubscriptionProfile()
        {
            CreateMap<CreateClubSubscriptionRequest, Context.ClubSubscription>();
            CreateMap<UpdateClubSubscriptionRequest, Context.ClubSubscription>();
            CreateMap<Context.ClubSubscription, ClubSubscriptionCreatedEvent>();
            CreateMap<Context.ClubSubscription, ClubSubscriptionUpdatedEvent>();
        }
    }
}
