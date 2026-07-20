using System;
using WZCNet.Entities;
using WZCNet.Models;
using WZCNet.Models.Creation;

namespace WZCNet.Services;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeBaseDto>> GetEmployeesAsync();
    Task<Result<EmployeeBaseDto>> GetEmployeeDetailsFromIdAsync(int id);
    Task<Result<Employee>> CreateEmployeeAsync(EmployeeCreationDTO input);

}
