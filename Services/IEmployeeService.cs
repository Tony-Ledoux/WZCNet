using System;
using WZCNet.Entities;
using WZCNet.Models;
using WZCNet.Models.Creation;

namespace WZCNet.Services;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeBaseDto>> GetEmployeesAsync();
    Task<EmployeeBaseDto?> CreateEmployeeAsync(EmployeeCreationDTO input);
}
