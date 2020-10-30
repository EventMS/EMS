using System;
using System.Threading.Tasks;
using EMS.Club_Service_Services.API;

namespace EMS.Payment_Services.API.GraphQlQueries
{
    public interface IPaymentService
    {
        public Task<float?> CalculateEventPriceForUserAsync(Guid eventId,
            [CurrentUserGlobalState] CurrentUser currentUser);
    }
}