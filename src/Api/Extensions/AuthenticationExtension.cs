using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace WZCNet.src.Api.Extensions;
public static class AuthenticationExtension
{
    public static IServiceCollection AddApplicationAuthentication(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = configuration["AppSettings:Issuer"],
        ValidateAudience = true,
        ValidAudience = configuration["AppSettings:Audience"],
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration["AppSettings:Token"]!)),
        ValidateIssuerSigningKey = true
    };
});
        
        return services;
    }
}