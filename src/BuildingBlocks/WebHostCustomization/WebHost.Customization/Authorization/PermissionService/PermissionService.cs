﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Serilog;

namespace EMS.TemplateWebHost.Customization.StartUp
{
    /// <summary>
    /// Client implementation for Permission Service as REST.
    /// </summary>
    public class PermissionService
    {
        private readonly HttpClient _client;

        public PermissionService(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> GetPermissions(Guid clubId, string role, string token)
        {
            _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(token);
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