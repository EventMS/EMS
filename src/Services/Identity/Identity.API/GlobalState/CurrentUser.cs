namespace Identity.API
{
    public class CurrentUser
    {
        public string UserId { get; }

        public CurrentUser(string userId)
        {
            UserId = userId;
        }
    }
}