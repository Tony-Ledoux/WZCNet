
using WZCNet.src.Application.DTOs;
using WZCNet.src.Application.DTOs.Creation;
using WZCNet.src.Domain.Common;
using WZCNet.src.Domain.Entities;

namespace WZCNet.src.Application.Services;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeBaseDto>> GetEmployeesAsync();
    Task<Result<EmployeeWithAddressDto>> GetEmployeeDetailsFromIdAsync(int id);
    Task<Result<Employee>> CreateEmployeeAsync(EmployeeCreationDTO input);

}
