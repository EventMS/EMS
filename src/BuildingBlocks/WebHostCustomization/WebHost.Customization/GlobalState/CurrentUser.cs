using System;
using System.Collections.Generic;
using EMS.TemplateWebHost.Customization.StartUp;

namespace EMS.Club_Service_Services.API
{
    /// <summary>
    /// The current user objekt that gets exposed as global accessable on GraphQL queries based on Bearer token. 
    /// </summary>
    public class CurrentUser
    {
        public Guid UserId { get; }
        public List<ClubPermission> ClubPermissions { get; }

        public CurrentUser(Guid userId, List<ClubPermission> clubPermissions)
        {
            UserId = userId;
            ClubPermissions = clubPermissions;
        }


        public Guid? GetSubscriptionIn(Guid clubId)
        {
            var club = ClubPermissions.Find(club => club.ClubId == clubId);
            return club?.SubscriptionId;
        }
    }
}