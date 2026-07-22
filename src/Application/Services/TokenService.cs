using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WZCNet.src.Application.DTOs;
using WZCNet.src.Application.Interfaces;
using WZCNet.src.Domain.Entities;

namespace WZCNet.src.Application.Services;

public class TokenService(IConfiguration configuration) : ITokenService
{
    public Task<string> CreateBearerToken(TokenClaimsDTO user)
    {
         var claims = new List<Claim>
            {
              new Claim(ClaimTypes.Name ,user.UserName) 
            };
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!)
            );
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );
            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(tokenDescriptor));
    }
}
