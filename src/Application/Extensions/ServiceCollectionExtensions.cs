using WZCNet.src.Application.Interfaces;
using WZCNet.src.Application.Services;

namespace WZCNet.src.Application.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}