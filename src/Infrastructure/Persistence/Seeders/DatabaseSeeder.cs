using System;
using Microsoft.AspNetCore.Identity;
using WZCNet.src.Domain.Entities;
using WZCNet.src.Domain.Entities.EmployeeAggregate;
using WZCNet.src.Infrastructure.Persistence.Contexts;

namespace WZCNet.src.Infrastructure.Persistence.Seeders;

public class DatabaseSeeder : IDatabaseSeeder
{
    private readonly WZCNetDbContext _context;
    private readonly IPasswordHasher<AppUser> _passwordHasher;

    public DatabaseSeeder(WZCNetDbContext context, IPasswordHasher<AppUser> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task SeedAsync()
    {
        if (!_context.Employees.Any())
        {
            await SeedEmployees();
        }
        if (!_context.AppUsers.Any())
        {
            
        }

        await _context.SaveChangesAsync();
    }

    private async Task SeedEmployees()
    {
        var addresses = new List<EmployeeAddress>
        {
            EmployeeAddress.Create("Sint Jobbaan","34","2390","Malle",null).Value
        };
        var employee = Employee.Create("Tony","Ledoux",DateOnly.Parse("1983-08-05"),"123456",addresses);
        if (employee.IsSuccess)
        {
            _context.Employees.Add(employee.Value);
        }
    }
    


}
