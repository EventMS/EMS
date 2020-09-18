using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using EMS.BuildingBlocks.IntegrationEventLogEF;
using EMS.BuildingBlocks.IntegrationEventLogEF.Utilities;
using EMS.Events;
using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Execution;
using Identity.API.Context.Models;
using Identity.API.Controllers;
using Identity.API.Data;
using Identity.API.Services;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Serilog;
using TemplateWebHost.Customization.IntegrationEventService;

namespace EMS.Events
{
    public class ValueEntered : IntegrationEvent
    {
        public string Value { get; set; }
        public string Value2 { get; set; }
    }
}

namespace Identity.API.GraphQlQueries
{
    public class IdentityMutations
    {
        private readonly ApplicationDbContext _context;
        private readonly IIntegrationEventService _integrationEventService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtService _jwtService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IPublishEndpoint _publishEndpoint;

        public IdentityMutations(ApplicationDbContext context, IIntegrationEventService template1IntegrationEventService, UserManager<ApplicationUser> userManager, JwtService jwtService, SignInManager<ApplicationUser> signInManager, IHttpContextAccessor contextAccessor, IPublishEndpoint publishEndpoint)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); ;
            _integrationEventService = template1IntegrationEventService ?? throw new ArgumentNullException(nameof(template1IntegrationEventService));
            _userManager = userManager;
            _jwtService = jwtService;
            _signInManager = signInManager;
            _contextAccessor = contextAccessor;
            _publishEndpoint = publishEndpoint;
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
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
            var user = new ApplicationUser()
            {
                Name = request.Name,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                UserName = request.Email
            };

            var evt = new UserCreatedIntegrationEvent(user.Id, user.Name);
            //Only do it this way if you have no direct control on context. Otherwise just do it like normally before. 
            await _integrationEventService.SaveEventAndDbContextChangesAsync(evt, async () =>
            {
                var result = await _userManager.CreateAsync(user, request.Password);
                // var @event = new Template1CreatedIntegrationEvent(item.Id, item.Name);
                // await _integrationEventService.SaveEventAndDbContextChangesAsync(@event);
                //  await _integrationEventService.PublishThroughEventBusAsync(@event);
                if (EnumerableExtensions.Any(result.Errors))
                {
                    throw new QueryException(
                        ErrorBuilder.New()
                            .SetMessage("The provided input is invalid")
                            .SetCode("ID_UNKNOWN")
                            .Build());
                }
            });
            await _integrationEventService.PublishThroughEventBusAsync(evt);

            var response = new Response()
            {
                User = user,
                Token = _jwtService.GenerateJwtToken(user.Id, user.Email)
            };
            return response;
        }

        [Authorize]
        public async Task<ApplicationUser> EditUserAsync(EditUserRequest request, [CurrentUserGlobalState] CurrentUser currentUser)
        {
            ApplicationUser user = await _context.Users.SingleAsync(applicationUser => applicationUser.Id == currentUser.UserId);

            user.Name = request.Name;
            user.PhoneNumber = request.PhoneNumber;
            _context.Update(user);

            var evt = new UserUpdatedIntegrationEvent(user.Id, user.Name);
            await _integrationEventService.SaveEventAndDbContextChangesAsync(evt);
            await _integrationEventService.PublishThroughEventBusAsync(evt);
            return user;
        }
    }
}