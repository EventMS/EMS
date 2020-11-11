using System.Threading.Tasks;
using EMS.Events;
using EMS.Payment_Services.API.Context;
using EMS.Payment_Services.API.Context.Model;
using MassTransit;
using Stripe;

namespace EMS.Payment_Services.API.Events
{
    public class ClubSubscriptionCreatedEventConsumer :
        IConsumer<ClubSubscriptionCreatedEvent>
    {
        private readonly PaymentContext _paymentContext;
        private readonly StripeService _stripeService;

        public ClubSubscriptionCreatedEventConsumer(PaymentContext paymentContext, StripeService stripeService)
        {
            _paymentContext = paymentContext;
            _stripeService = stripeService;
        }

        public async Task Consume(ConsumeContext<ClubSubscriptionCreatedEvent> context)
        {
            var subscription = await _paymentContext.ClubSubscriptions.FindAsync(context.Message.ClubSubscriptionId);

            if (subscription == null)
            {
                Product product = _stripeService.CreateProduct(context.Message.ClubId, context.Message.Name);
                Price price = _stripeService.CreatePrice(context.Message.Price, product);
                subscription = new ClubSubscription()
                {
                    ClubSubscriptionId = context.Message.ClubSubscriptionId,
                    ClubId = context.Message.ClubId,
                    StripePriceId = price.Id,
                    StripeProductId = product.Id,
                };
                await _paymentContext.ClubSubscriptions.AddAsync(subscription);
                await _paymentContext.SaveChangesAsync();
            }
        }
    }
}