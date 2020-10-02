using AutoMapper;
using EMS.Events;
using Subscription.API.GraphQlQueries.Request;
using Subscription.API.IntegrationEvents;

namespace Subscription.API.Mapper
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
