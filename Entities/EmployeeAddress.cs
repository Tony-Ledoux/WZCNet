using System;
using System.ComponentModel.DataAnnotations.Schema;
using WZCNet.Models;

namespace WZCNet.Entities;

public class EmployeeAddress: BaseEntity
{
    public int EmployeeId {get;set;}
    public string StreetName {get;set;}
    public string HouseNumber {get;set;}
    public string ZipCode {get;set;}
    public string Municipality {get;set;}
    public DateOnly? Until {get;set;}

    //navigational property
    [ForeignKey(nameof(EmployeeId))]
    public Employee Employee {get;set;}

    private EmployeeAddress(){}

    public static Result<EmployeeAddress> Create(Employee employee, string streetName, string houseNumber, string zipCode, string municipality, DateOnly? until)
    {
        if (employee is null) return Result<EmployeeAddress>.Failure("Werknemer is verplicht.");

        ArgumentException.ThrowIfNullOrEmpty(streetName, nameof(streetName));
        ArgumentException.ThrowIfNullOrEmpty(houseNumber, nameof(houseNumber));
        ArgumentException.ThrowIfNullOrEmpty(zipCode, nameof(zipCode));
        ArgumentException.ThrowIfNullOrEmpty(municipality, nameof(municipality));
        if (until.HasValue)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            if (until.Value < today.AddYears(-100) || until.Value > today.AddYears(100))
                return Result<EmployeeAddress>.Failure("'Until' datum moet tussen 100 jaar geleden en 100 jaar in de toekomst liggen.");
        }

        return Result<EmployeeAddress>.Success(new()
        {
            Employee = employee,
            StreetName = streetName.Trim(),
            HouseNumber = houseNumber.Trim(),
            ZipCode = zipCode.Trim(),
            Municipality=municipality.Trim(),
            Until = until
        });

    }
}
