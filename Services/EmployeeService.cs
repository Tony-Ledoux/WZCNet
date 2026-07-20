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
    //TODO generate a pinHash
    public async Task<Result<Employee>> CreateEmployeeAsync(EmployeeCreationDTO input)
    {
        bool exists = await _db.Employees.AsNoTracking().AnyAsync(
            e=> e.FirstName.ToLower() == input.FirstName.ToLower() &&
            e.LastName.ToLower() == input.LastName.ToLower() &&
            e.DateOfBirth == input.DateOfBirth
        );
        if (exists) return Result<Employee>.Failure($"Een werknemer genaamd {input.FirstName}, {input.LastName} geboren op {input.DateOfBirth:dd-MM-yyyy} bestaat al");
        
        
        var domain = Employee.Create(
            input.FirstName,
            input.LastName,
            input.DateOfBirth,
            "951",
            emp=>(input.Addresses??[]).Select(a=>EmployeeAddress.Create(emp,a.StreetName,a.HouseNumber,a.ZipCode,a.Municipality,a.Until)));
        if(!domain.IsSuccess) return Result<Employee>.Failure(domain.Error!);
        _db.Employees.Add(domain.Value);
        await _db.SaveChangesAsync();
        //mapping if needed
        return Result<Employee>.Success(domain.Value);

    }

    public async Task<Result<EmployeeBaseDto>> GetEmployeeDetailsFromIdAsync(int id)
    {
        var employee = await _db.Employees.Include(e => e.EmployeeAddresses).FirstOrDefaultAsync(e => e.Id == id);
        if(employee==null) return Result<EmployeeBaseDto>.Failure($"medewerker met id {id} niet gevonden");
        EmployeeBaseDto e = new()
        {
            Id = employee.Id,
            FirstName=employee.FirstName,
            LastName = employee.LastName,
            DateOfBirth = employee.DateOfBirth
        };
        return Result<EmployeeBaseDto>.Success(e);
    }

    public async Task<IEnumerable<EmployeeBaseDto>> GetEmployeesAsync()
    {
        var employee = await _db.Employees.Include(e=>e.Pin).Select(e=> new EmployeeBaseDto
        {
            Id = e.Id,
            FirstName = e.FirstName,
            LastName = e.LastName,
            DateOfBirth = e.DateOfBirth
        }).ToListAsync();
        return employee;
    }
}
