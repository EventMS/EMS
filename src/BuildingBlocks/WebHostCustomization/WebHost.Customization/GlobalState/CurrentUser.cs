using System;

namespace EMS.Club_Service_Services.API
{
    public class CurrentUser
    {
        public Guid UserId { get; }

        public CurrentUser(Guid userId)
        {
            UserId = userId;
        }
    }
}