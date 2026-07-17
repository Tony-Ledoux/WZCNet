using System;
using Microsoft.EntityFrameworkCore;
using WZCNet.Contexts;
using WZCNet.Entities;
using WZCNet.Exeptions;
using WZCNet.Models;
using WZCNet.Models.Creation;

namespace WZCNet.Services;

public class EmployeeService(WZCNetDbContext context): IEmployeeService
{
    private readonly WZCNetDbContext _db = context;

    public async Task<EmployeeBaseDto?> CreateEmployeeAsync(EmployeeCreationDTO input)
    {
        bool exists = await _db.Employees.AnyAsync(
            e=> e.FirstName.ToLower() == input.FirstName.ToLower() &&
            e.LastName.ToLower() == input.LastName.ToLower() &&
            e.DateOfBirth == input.DateOfBirth
        );
        if (exists)
        {
            throw new ConflictExeption($"Een werknemer met genaamd {input.FirstName} {input.LastName} geboren op {input.DateOfBirth} bestaat al");
        }
        var employee = new Employee
        {
            FirstName = input.FirstName,
            LastName = input.LastName,
            DateOfBirth=input.DateOfBirth
        };
        _db.Employees.Add(employee);
        await _db.SaveChangesAsync();
        return new EmployeeBaseDto
        {
            Id = employee.Id,
            FirstName=employee.FirstName,
            LastName = employee.LastName,
            DateOfBirth=employee.DateOfBirth
        };

    }

    public async Task<IEnumerable<EmployeeBaseDto>> GetEmployeesAsync()
    {
        var employee = await _db.Employees.Select(e=> new EmployeeBaseDto
        {
            Id = e.Id,
            FirstName = e.FirstName,
            LastName = e.LastName,
            DateOfBirth = e.DateOfBirth
        }).ToListAsync();
        return employee;
    }
}
