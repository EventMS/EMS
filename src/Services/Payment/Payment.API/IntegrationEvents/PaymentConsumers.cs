using System;
using System.Threading.Tasks;
using EMS.Club_Service_Services.API;
using EMS.Events;
using EMS.Payment_Services.API.Context;
using EMS.Payment_Services.API.Context.Model;
using MassTransit;
using Serilog;

namespace EMS.Payment_Services.API.Events
{
    public class UserCreatedEventConsumer :
            IConsumer<UserCreatedEvent>
    {
        private readonly PaymentContext _paymentContext;
        private readonly StripeService _stripeService;

        public UserCreatedEventConsumer(PaymentContext paymentContext, StripeService stripeService)
        {
            _paymentContext = paymentContext;
            _stripeService = stripeService;
        }

        public async Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            var user = await _paymentContext.Users.FindAsync(context.Message.UserId);

            if (user == null)
            {
                var customer = _stripeService.CreateCustomer(context.Message.Email);
                user = new User()
                {
                    UserId = new Guid(context.Message.UserId),
                    StripeUserId = customer.Id
                };
                await _paymentContext.Users.AddAsync(user);
                await _paymentContext.SaveChangesAsync();
            }
        }
    }
}
