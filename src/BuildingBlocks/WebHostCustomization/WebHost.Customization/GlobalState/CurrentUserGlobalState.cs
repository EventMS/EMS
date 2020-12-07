using HotChocolate;

namespace EMS.Club_Service_Services.API
{
    /// <summary>
    /// The exposed GlobalState Attribute
    /// </summary>
    public class CurrentUserGlobalState : GlobalStateAttribute
    {
        public CurrentUserGlobalState() : base("currentUser")
        {
        }
    }
}