using System;
using System.Threading.Tasks;
using EMS.BuildingBlocks.EventLogEF;
using EMS.Club_Service_Services.API;
using EMS.Events;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Execution;
using EMS.Identity_Services.API.Context.Models;
using EMS.Identity_Services.API.Controllers;
using EMS.Identity_Services.API.Data;
using EMS.Identity_Services.API.Services;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using EMS.TemplateWebHost.Customization.EventService;
using Stripe;
using Event = EMS.BuildingBlocks.EventLogEF.Event;
using EMS.Identity_Services.API.GraphQlQueries;

namespace EMS.Events
{
    public class ValueEntered : Event
    {
        public string Value { get; set; }
        public string Value2 { get; set; }
    }
}

public class StripeService
{
    public Customer CreateCustomer(CreateUserRequest request)
    {
        var options = new CustomerCreateOptions
        {
            Email = request.Email,
        };
        var service = new CustomerService();
        var customer = service.Create(options);
        return customer;
    }

}

namespace EMS.Identity_Services.API.GraphQlQueries
{
    public class IdentityMutations
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventService _eventService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtService _jwtService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly StripeService _stripeService;

        public IdentityMutations(ApplicationDbContext context, IEventService template1EventService, UserManager<ApplicationUser> userManager, JwtService jwtService, SignInManager<ApplicationUser> signInManager, IHttpContextAccessor contextAccessor, IPublishEndpoint publishEndpoint, StripeService stripeService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); ;
            _eventService = template1EventService ?? throw new ArgumentNullException(nameof(template1EventService));
            _userManager = userManager;
            _jwtService = jwtService;
            _signInManager = signInManager;
            _contextAccessor = contextAccessor;
            _publishEndpoint = publishEndpoint;
            _stripeService = stripeService;
        }


        public async Task<Response> LoginUserAsync(LoginUserRequest request)
        {
            //Some explicit transaction because user manager is out of ordinary and autosaves otherwise in another scope. 
            var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, false);
            if (!result.Succeeded)
            {
                throw new QueryException(
                    ErrorBuilder.New()
                        .SetMessage("Cannot login, invalid credentials")
                        .SetCode("ID_UNKNOWN")
                        .Build());
            }

            var user = await _userManager.FindByEmailAsync(request.Email);

            var response = new Response()
            {
                User = user,
                Token = _jwtService.GenerateJwtToken(user.Id, user.Email)
            };
            return response;
        }

        public async Task<Response> CreateUserAsync(CreateUserRequest request)
        {
            //Create the customer
            if (await _userManager.FindByEmailAsync(request.Email) != null)
            {
                throw new QueryException("Duplicate email");
            };

            Customer customer = _stripeService.CreateCustomer(request);

            var user = new ApplicationUser()
            {
                Name = request.Name,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                UserName = request.Email,
                StripeCustomerId = customer.Id
            };

            var evt = new UserCreatedEvent()
            {
                Name = user.Name,
                UserId = user.Id,
                StripeCustomerId = customer.Id
            };

            //Only do it this way if you have no direct control on context. Otherwise just do it like normally before. 
            await _eventService.SaveEventAndDbContextChangesAsync(evt, async () =>
            {
                var result = await _userManager.CreateAsync(user, request.Password);
                // var @event = new Template1CreatedEvent(item.Id, item.Name);
                // await _eventService.SaveEventAndDbContextChangesAsync(@event);
                //  await _eventService.PublishEventAsync(@event);
                if (EnumerableExtensions.Any(result.Errors))
                {
                    throw new QueryException(
                        ErrorBuilder.New()
                            .SetMessage("The provided input is invalid")
                            .SetCode("ID_UNKNOWN")
                            .Build());
                }
            });
            await _eventService.PublishEventAsync(evt);

            var response = new Response()
            {
                User = user,
                Token = _jwtService.GenerateJwtToken(user.Id, user.Email)
            };
            return response;
        }


        [HotChocolate.AspNetCore.Authorization.Authorize]

        public async Task<ApplicationUser> EditUserAsync(EditUserRequest request, [CurrentUserGlobalState] CurrentUser currentUser)
        {
            ApplicationUser user = await _context.Users.SingleAsync(applicationUser => applicationUser.Id == currentUser.UserId.ToString());

            user.Name = request.Name;
            user.PhoneNumber = request.PhoneNumber;
            _context.Update(user);

            var evt = new UserUpdatedEvent(user.Id, user.Name);
            await _eventService.SaveEventAndDbContextChangesAsync(evt);
            await _eventService.PublishEventAsync(evt);
            return user;
        }
    }
}