using Microsoft.EntityFrameworkCore;
using WZCNet.src.Application.DTOs;
using WZCNet.src.Application.DTOs.Creation;
using WZCNet.src.Domain.Common;
using WZCNet.src.Domain.Entities;
using WZCNet.src.Domain.ValueObjects;
using WZCNet.src.Infrastructure.Persistence.Contexts;

namespace WZCNet.src.Application.Services;

public class EmployeeService(WZCNetDbContext context): IEmployeeService
{
    private readonly WZCNetDbContext _db = context;
    //TODO generate a pinHash
    public async Task<Result<Employee>> CreateEmployeeAsync(EmployeeCreationDTO input)
    {
        bool exists = await _db.Employees.AsNoTracking().AnyAsync(
            e=> e.Name.First.ToLower() == input.FirstName.ToLower() &&
            e.Name.Last.ToLower() == input.LastName.ToLower() &&
            e.DateOfBirth == input.DateOfBirth
        );
        if (exists) return Result<Employee>.Failure($"Een werknemer genaamd {input.FirstName}, {input.LastName} geboren op {input.DateOfBirth:dd-MM-yyyy} bestaat al");
        
        var addresses = new List<Address>();
        foreach (var addr in input.Addresses ?? [])
        {
            var result = Address.Create(addr.StreetName,addr.HouseNumber,addr.ZipCode,addr.Municipality,addr.Until);
            if(!result.IsSuccess) return Result<Employee>.Failure(result.Error);
            addresses.Add(result.Value);
        }
        
        var domain = Employee.Create(
            input.FirstName,
            input.LastName,
            input.DateOfBirth,
            "951",
            addresses);
        if(!domain.IsSuccess) return Result<Employee>.Failure(domain.Error!);
        _db.Employees.Add(domain.Value);
        await _db.SaveChangesAsync();
        //mapping if needed
        return Result<Employee>.Success(domain.Value);

    }

    public async Task<Result<EmployeeWithAddressDto>> GetEmployeeDetailsFromIdAsync(int id)
    {
        var employee = await _db.Employees.FirstOrDefaultAsync(e => e.Id == id);
        if(employee==null) return Result<EmployeeWithAddressDto>.Failure($"medewerker met id {id} niet gevonden");
        EmployeeWithAddressDto e = new()
        {
            Id = employee.Id,
            FirstName=employee.Name.First,
            LastName = employee.Name.Last,
            DateOfBirth = employee.DateOfBirth,
            Addresses = [.. employee.Addresses.Select(a=>new EmployeeAddressDto{
                StreetName = a.StreetName,
                HouseNumber = a.HouseNumber,
                ZipCode = a.ZipCode,
                Municipality = a.Municipality,
                Until = a.Until
            })]
        };
        return Result<EmployeeWithAddressDto>.Success(e);
    }

    public async Task<IEnumerable<EmployeeBaseDto>> GetEmployeesAsync()
    {
        var employee = await _db.Employees.Include(e=>e.Pin).Select(e=> new EmployeeBaseDto
        {
            Id = e.Id,
            FirstName = e.Name.First,
            LastName = e.Name.Last,
            DateOfBirth = e.DateOfBirth
        }).ToListAsync();
        return employee;
    }
}
