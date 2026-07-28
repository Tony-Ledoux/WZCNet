using WZCNet.src.Application.Interfaces;
using WZCNet.src.Application.Interfaces.Repositories;
using WZCNet.src.Application.Services;
using WZCNet.src.Domain.Interfaces;
using WZCNet.src.Infrastructure.Persistence.Repositories;

namespace WZCNet.src.Application.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService,UserService>();
        services.AddScoped<IUnitOfWork,UnitOfWork>();
        services.AddScoped<IUserRepository,UserRepository>();
        services.AddScoped<IRefreshtokenRepository,RefreshTokenRepository>();

        return services;
    }
}