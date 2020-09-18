namespace GraphQL.API
{
    public class Content
    {
        public string OperationName { get; set; }
        public Request Variables { get; set; }
        public string Query { get; set; }
    }

    public class Request
    {
        public string ClubId { get; set; }
        public string EventId { get; set; }
    }
}