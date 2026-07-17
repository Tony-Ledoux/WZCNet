using System;
using WZCNet.Entities;
using WZCNet.Models;
using WZCNet.Models.Creation;

namespace WZCNet.Services;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeBaseDto>> GetEmployeesAsync();
    Task<EmployeeWithAddressDto?> GetEmployeeDetailsFromIdAsync(int id);
    Task<EmployeeBaseDto?> CreateEmployeeAsync(EmployeeCreationDTO input);

}
