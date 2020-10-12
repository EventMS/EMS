using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Serilog;

namespace EMS.TemplateWebHost.Customization.StartUp
{
    public class PermissionService
    {
        private readonly HttpClient _client;

        public PermissionService(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> GetPermissions(Guid clubId, string role)
        {
            var response = await _client.GetAsync("/api/permission/" + clubId+"/"+role);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetFatToken()
        {
            var response = await _client.GetAsync("/api/permission");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}