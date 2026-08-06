using WZCNet.src.Application.Interfaces;
using WZCNet.src.Application.Interfaces.Repositories;
using WZCNet.src.Application.Services;
using WZCNet.src.Domain.Interfaces;
using WZCNet.src.Infrastructure.Persistence.Repositories;
using WZCNet.src.Infrastructure.Persistence.Seeders;

namespace WZCNet.src.Application.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService,UserService>();
        services.AddScoped<IEmployeeService,EmployeeService>();
        services.AddScoped<IUnitOfWork,UnitOfWork>();
        services.AddScoped<IUserAccountRepository,UserAccountRepository>();
        services.AddScoped<IEmployeeRepository,EmployeeRepository>();
        

        return services;
    }
}