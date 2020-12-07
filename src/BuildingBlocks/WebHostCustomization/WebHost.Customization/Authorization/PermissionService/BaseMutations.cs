using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace EMS.TemplateWebHost.Customization.StartUp
{
    /// <summary>
    /// Base mutations service that implement basic authorisations. 
    /// </summary>
    public class BaseMutations
    {
        protected readonly IAuthorizationService _authorizationService;

        public BaseMutations(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        private async Task HasRoleIn(Guid clubId, string Role)
        {
            await _authorizationService
                .AuthorizeAsync(null, clubId, Role);
        }

        protected async Task IsAdminIn(Guid clubId)
        {
            await HasRoleIn(clubId, "Admin");
        }

        protected async Task IsInstructorIn(Guid clubId)
        {
            await HasRoleIn(clubId, "Instructor");
        }

        protected async Task IsMemberIn(Guid clubId)
        {
            await HasRoleIn(clubId, "Member");
        }
    }
}