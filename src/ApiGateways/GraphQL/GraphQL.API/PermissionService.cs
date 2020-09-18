using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Serilog;

namespace GraphQL.API
{
    public class PermissionService
    {
        private readonly HttpClient _client;

        public PermissionService(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> GetPermissions(Content body)
        {
            HttpContent content = new StringContent(JsonSerializer.Serialize(body),
                Encoding.UTF8,
        "application/json");
            var response = await _client.PostAsync("/api/permission", content);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}