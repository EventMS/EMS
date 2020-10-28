using System;
using System.IO;
using System.Threading.Tasks;
using EMS.Events;
using EMS.TemplateWebHost.Customization.EventService;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Stripe;

namespace EMS.PaymentWebhook_Services.API.GraphQlQueries
{
    [Route("webhook")]
    [ApiController]
    public class WebhookController : Controller
    {
        private readonly IEventService _eventService;

        public WebhookController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [Route("event")]
        [HttpPost]
        public async Task<IActionResult> eventCheat(EventCheatRequest request)
        {
            var e = new SignUpEventSuccess()
            {
                UserId = request.UserId,
                EventId = request.EventId
            };
            Log.Information("User: " + e.UserId + " signed up to eventId: " + e.EventId);
            await _eventService.SaveEventAndDbContextChangesAsync(e);
            await _eventService.PublishEventAsync(e);
            return Ok();
        }



        [Route("sub")]
        [HttpPost]
        public async Task<IActionResult> subCheat(SubCheatRequest request)
        {
            var e = new SignUpSubscriptionSuccessEvent()
            {
                UserId = request.UserId,
                ClubSubscriptionId = request.ClubSubscriptionId
            };
            Log.Information("User: " + e.UserId + " signed up to subscription: " + e.ClubSubscriptionId);
            await _eventService.SaveEventAndDbContextChangesAsync(e);
            await _eventService.PublishEventAsync(e);
            return Ok();
        }



        [HttpPost]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            Log.Information(json);
            try
            {
                var stripeEvent = EventUtility.ParseEvent(json);
                if (stripeEvent.Type == Stripe.Events.PaymentIntentSucceeded)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    if (paymentIntent.Metadata.Count == 2)
                    {
                        var e = new SignUpEventSuccess()
                        {
                            UserId = new Guid(paymentIntent.Metadata["UserId"]),
                            EventId = new Guid(paymentIntent.Metadata["EventId"])
                        };
                        Log.Information("User: " + e.UserId + " signed up to eventId: " + e.EventId);
                        await _eventService.SaveEventAndDbContextChangesAsync(e);
                        await _eventService.PublishEventAsync(e);
                    }
                }
                else if (stripeEvent.Type == Stripe.Events.CustomerSubscriptionCreated)
                {
                    var sub = stripeEvent.Data.Object as Subscription;
                    if (sub.Metadata.Count == 2)
                    {
                        var e = new SignUpSubscriptionSuccessEvent()
                        {
                            UserId = new Guid(sub.Metadata["UserId"]),
                            ClubSubscriptionId = new Guid(sub.Metadata["ClubSubscriptionId"])
                        };
                        Log.Information("User: " + e.UserId + " signed up to subscription: " + e.ClubSubscriptionId);
                        await _eventService.SaveEventAndDbContextChangesAsync(e);
                        await _eventService.PublishEventAsync(e);
                    }
                }
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }
                return Ok();
            }
            catch
            {
            }

            return BadRequest();
        }
    }
}