using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using EMS.Club_Service.API.Context;
using EMS.Club_Service.API.Context.Model;
using EMS.Club_Service.API.Controllers.Request;
using EMS.Club_Service_Services.API;
using EMS.Events;
using EMS.TemplateWebHost.Customization;
using EMS.TemplateWebHost.Customization.EventService;
using EMS.TemplateWebHost.Customization.StartUp;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace EMS.Club_Service.API.GraphQlQueries
{
    /// <summary>
    /// Update club mutations
    /// </summary>
    public class ClubMutations : BaseMutations
    {
        private readonly ClubContext _context;
        private readonly IMapper _mapper;
        private readonly IEventService _eventService;

        public ClubMutations(ClubContext context, IEventService template1EventService, IMapper mapper, IAuthorizationService authorizationService) : base(authorizationService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context)); ;
            _eventService = template1EventService ?? throw new ArgumentNullException(nameof(template1EventService));
            _mapper = mapper;
        }

        [HotChocolate.AspNetCore.Authorization.Authorize]
        public async Task<Club> UpdateClubAsync(Guid clubId, UpdateClubRequest request)
        {
            await IsAdminIn(clubId);
            var item = await _context.Clubs.FindOrThrowAsync(clubId);

            _mapper.Map(request, item);
            _context.Clubs.Update(item);

            var @event = _mapper.Map<ClubUpdatedEvent>(item);
            await _eventService.SaveEventAndDbContextChangesAsync(@event);
            await _eventService.PublishEventAsync(@event);

            return item;
        }

        [HotChocolate.AspNetCore.Authorization.Authorize]
        public async Task<Club> CreateClubAsync(CreateClubRequest request, [CurrentUserGlobalState] CurrentUser currentUser)
        {
            var item = _mapper.Map<Club>(request);
            item.AdminId = currentUser.UserId;
            _context.Clubs.Add(item);

            var @event = _mapper.Map<ClubCreatedEvent>(item);
            @event.Locations = request.Locations;
            await _eventService.SaveEventAndDbContextChangesAsync(@event);
            await _eventService.PublishEventAsync(@event);

            return item;
        }
    }
}
