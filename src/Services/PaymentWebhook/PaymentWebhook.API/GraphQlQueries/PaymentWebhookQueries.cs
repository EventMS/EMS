using System.Linq;
using EMS.PaymentWebhook_Services.API.Context;
using EMS.PaymentWebhook_Services.API.Context.Model;

namespace EMS.PaymentWebhook_Services.API.GraphQlQueries
{
    public class PaymentWebhookQueries
    {
        private readonly PaymentWebhookContext _context;
        public PaymentWebhookQueries(PaymentWebhookContext context)
        {
            _context = context;
        }

        public IQueryable<PaymentWebhook> PaymentWebhooks => _context.PaymentWebhooks.AsQueryable();
    }
}
