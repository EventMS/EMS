using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using EMS.Permission_Services.API.Context.Model;
using EMS.TemplateWebHost.Customization.StartUp;
using Newtonsoft.Json;
using Serilog;

namespace EMS.Permission_Services.API.Services
{
    public class JwtService
    {
        public JwtService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }


        public string GenerateJwtToken(User user, List<Role> userRoles)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = Configuration.GetValue<string>("SecurityKey");
            var key = Encoding.ASCII.GetBytes(securityKey);

            var subject = new ClaimsIdentity();
            //Attach information in token here somewhere
            subject.AddClaim(new Claim("id", user.UserId.ToString()));
            subject.AddClaim(new Claim("stripeCustomerId", user.StripeCustomerId));

            if (userRoles == null)
            {
                userRoles = new List<Role>();
            }

            var clubPermissons = userRoles.Select(role => new ClubPermission()
            {
                ClubId = role.ClubId,
                UserRole = role.UserRole,
                SubscriptionId = role.ClubSubscriptionId
            }).ToList();
            subject.AddClaim(new Claim("ClubPermissionsClaim", JsonConvert.SerializeObject(clubPermissons)));

                var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = null,
                Audience = null,
                Subject = subject,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
