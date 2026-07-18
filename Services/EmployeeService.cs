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
            throw new ConflictExeption($"Een werknemer genaamd {input.FirstName}, {input.LastName} geboren op {input.DateOfBirth:dd-MM-yyyy} bestaat al");
        }
        var employee = new Employee
        {
            FirstName = input.FirstName,
            LastName = input.LastName,
            DateOfBirth=input.DateOfBirth
        };
        _db.Employees.Add(employee);
        if(input.Addresses != null)
        {
            foreach (var address in input.Addresses)
            {
                var a = new EmployeeAddress()
                {
                  StreetName = address.StreetName,
                  HouseNumber = address.HouseNumber,
                  ZipCode = address.ZipCode,
                  Municipality = address.Municipality,
                  Until = address?.Until ?? null  
                };
                employee.EmployeeAddresses.Add(a);
            }
        }
        await _db.SaveChangesAsync();
        return new EmployeeBaseDto
        {
            Id = employee.Id,
            FirstName=employee.FirstName,
            LastName = employee.LastName,
            DateOfBirth=employee.DateOfBirth
        };

    }

    public async Task<EmployeeWithAddressDto?> GetEmployeeDetailsFromIdAsync(int id)
    {
        var employee = await _db.Employees.Include(e=>e.EmployeeAddresses).FirstOrDefaultAsync(e=> e.Id == id);
        if(employee==null) throw new NotFoundException($"medewerker met id {id} niet gevonden");
        return new EmployeeWithAddressDto
        {
            Id = employee.Id,
            FirstName=employee.FirstName,
            LastName = employee.LastName,
            DateOfBirth=employee.DateOfBirth,
            Addresses = [.. employee.EmployeeAddresses.Select(ea=> new EmployeeAddressDto{ 
                Id=ea.Id,
                EmployeeId = ea.EmployeeId,
                StreetName=ea.StreetName,
                HouseNumber = ea.HouseNumber,
                ZipCode = ea.ZipCode,
                Municipality = ea.Municipality,
                Until = ea.Until
                })]

        };
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
