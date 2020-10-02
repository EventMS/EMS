using HotChocolate;

namespace Club.API
{
    public class CurrentUserGlobalState : GlobalStateAttribute
    {
        public CurrentUserGlobalState() : base("currentUser")
        {
        }
    }
}