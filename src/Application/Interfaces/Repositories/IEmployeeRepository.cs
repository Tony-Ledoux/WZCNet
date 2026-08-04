using WZCNet.src.Domain.Entities;
using WZCNet.src.Domain.Entities.EmployeeAggregate;
using WZCNet.src.Domain.ValueObjects;

namespace WZCNet.src.Application.Interfaces.Repositories;

public interface IEmployeeRepository
{
    
    Task<Employee?> GetEmployeeByIdAsync(int id);
    
}
