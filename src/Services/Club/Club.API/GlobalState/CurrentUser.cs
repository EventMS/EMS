using System;

namespace Identity.API
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