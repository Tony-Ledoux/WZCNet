
using Microsoft.EntityFrameworkCore;
using WZCNet.src.Application.Interfaces.Repositories;
using WZCNet.src.Domain.Entities.EmployeeAggregate;
using WZCNet.src.Infrastructure.Persistence.Contexts;


namespace WZCNet.src.Infrastructure.Persistence.Repositories;

public class EmployeeRepository(WZCNetDbContext context) : IEmployeeRepository
{
    public async Task<Employee?> GetEmployeeByIdAsync(int id)
    {
        return await context.Employees.Include(e=>e.Pin).FirstOrDefaultAsync(e=>e.Id == id);
    }
    public async Task<Employee?> GetEmployeeByIdWithEmploymentHistoryAsync(int id)
{
    return await context.Employees
        .Include(e => e.Pin)
        .Include(e => e.EmploymentHistories)
        .FirstOrDefaultAsync(e => e.Id == id);
}
}
