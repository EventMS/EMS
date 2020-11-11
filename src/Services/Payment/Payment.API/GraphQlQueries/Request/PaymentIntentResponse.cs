namespace EMS.Payment_Services.API.GraphQlQueries
{
    public class PaymentIntentResponse{
        public string ClientSecret { get; set; }
        public float Price { get; set; }
    }
}