using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using EMS.Events;
using EMS.Permission_Services.API.Context;
using EMS.Permission_Services.API.Context.Model;
using EMS.Permission_Services.API.Controller;
using EMS.Permission_Services.API.Services;
using EMS.SharedTesting.Helper;
using EMS.TemplateWebHost.Customization.StartUp;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;


namespace EMS.Template1_Services.API.UnitTests.GraphQL
{
    [TestFixture]
    class JwtServiceTest : BaseMutationsSetupTests<PermissionContext>
    {
        
        #region Setup
        private JwtService jwt;

        [SetUp]
        public void SetUp()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            jwt = new JwtService(configuration);
        }
        #endregion


        [Test]
        public async Task GenerateJwtToken_UserIdTAttached_ContainsExpectedID()
        {
            var userId = Guid.NewGuid();
            var user = new User()
            {
                UserId = userId
            };
            List<Role> roles = new List<Role>();
            var token = jwt.GenerateJwtToken(user, roles);

            var tokenResult = new JwtSecurityTokenHandler().ReadJwtToken(token);
            Assert.That(tokenResult.Claims.ToList().Find(c => c.Type == "id" && c.Value == userId.ToString()), Is.Not.Null);
        }


        [Test]
        public async Task GenerateJwtToken_UserIdTAttached_ContainsExpectedRoles()
        {
            var userId = Guid.NewGuid();
            var user = new User()
            {
                UserId = userId
            };
            List<Role> roles = new List<Role>()
            {
                new Role()
                {
                    UserId = Guid.NewGuid(),
                    ClubSubscriptionId = Guid.NewGuid(),
                    ClubId = Guid.NewGuid(),
                    UserRole = "Member"
                }
            };
            var token = jwt.GenerateJwtToken(user, roles);

            var tokenResult = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var rolesResult = tokenResult.Claims.ToList().Find(c => c.Type == "ClubPermissionsClaim");
            Assert.That(roles, Is.Not.Null);
            var rolesOb = JsonConvert.DeserializeObject<List<ClubPermission>>(rolesResult.Value);

            Assert.That(rolesOb.First().ClubId, Is.EqualTo(roles.First().ClubId));
            Assert.That(rolesOb.First().SubscriptionId, Is.EqualTo(roles.First().ClubSubscriptionId));
            Assert.That(rolesOb.First().UserRole, Is.EqualTo(roles.First().UserRole));
        }

    }
}
