using HotChocolate;

namespace EMS.Club_Service_Services.API
{
    public class CurrentUserGlobalState : GlobalStateAttribute
    {
        public CurrentUserGlobalState() : base("currentUser")
        {
        }
    }
}