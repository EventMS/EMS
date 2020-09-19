﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Permission.API.Context.Model;
using Serilog;

namespace Permission.API.Services
{
    public class JwtService
    {
        public JwtService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }


        public string GenerateJwtToken(UserPermission userPermission)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = Configuration.GetValue<string>("SecurityKey");
            var key = Encoding.ASCII.GetBytes(securityKey);

            var subject = new ClaimsIdentity();
            //Attach information in token here somewhere
            subject.AddClaim(new Claim("id", userPermission.UserId.ToString()));

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
