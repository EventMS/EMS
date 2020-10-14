using AutoMapper;
using EMS.Events;
using EMS.PaymentWebhook_Services.API.Context.Model;

namespace EMS.PaymentWebhook_Services.API.Mapper
{
    public class PaymentWebhookProfile : Profile
    {
        public PaymentWebhookProfile()
        {
          //  CreateMap<CreatePaymentWebhookRequest, PaymentWebhook>();
          //  CreateMap<UpdatePaymentWebhookRequest, PaymentWebhook>();
          //  CreateMap<PaymentWebhook, PaymentWebhookCreatedEvent>();
           // CreateMap<PaymentWebhook, PaymentWebhookUpdatedEvent>();
            //CreateMap<PaymentWebhook, PaymentWebhookDeletedEvent>();
        }
    }
}
