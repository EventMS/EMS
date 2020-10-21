using System;
using System.Collections.Generic;
using EMS.TemplateWebHost.Customization.StartUp;

namespace EMS.Club_Service_Services.API
{

    public class CurrentUser
    {
        public Guid UserId { get; }
        public string StripeCustomerId { get; }
        public List<ClubPermission> ClubPermissions { get; }

        public CurrentUser(Guid userId, List<ClubPermission> clubPermissions, string stripeCustomerId)
        {
            UserId = userId;
            ClubPermissions = clubPermissions;
            StripeCustomerId = stripeCustomerId;
        }


        public Guid? GetSubscriptionIn(Guid clubId)
        {
            var club = ClubPermissions.Find(club => club.ClubId == clubId);
            return club?.SubscriptionId;
        }
    }
}