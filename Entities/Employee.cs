using System;
using WZCNet.Exeptions;
using WZCNet.Models;

namespace WZCNet.Entities;

public class Employee: BaseEntity
{
    public string FirstName {get;set;}
    public string LastName {get;set;}
    public DateOnly DateOfBirth {get;set;}

    // navigation Property

    public EmployeeAuthentication? Pin {get;set;}

    public ICollection<EmployeeAddress> EmployeeAddresses {get;set;} = [];
    public ICollection<EmployeeContact> EmployeeContacts {get;set;}=[];
    public ICollection<EmploymentHistory> EmploymentHistories {get;set;}=[];

    public ICollection<EmployeeComment> CommentsAuthored {get;set;}=[];
    public ICollection<EmployeeComment> CommentsRecieved {get;set;}= [];
    public ICollection<EmployeePermission> PersonalPermissions {get;set;}= [];

    private Employee(){}
    public static Result<Employee> Create(string FirstName, string lastName, DateOnly dateOfBirth, string pinHash, Func<Employee, IEnumerable<Result<EmployeeAddress>>> addressFactory)
    {
        ArgumentException.ThrowIfNullOrEmpty(FirstName, nameof(FirstName));
        ArgumentException.ThrowIfNullOrEmpty(lastName,nameof(lastName));
        ArgumentException.ThrowIfNullOrWhiteSpace(pinHash,nameof(pinHash));
        ArgumentNullException.ThrowIfNull(addressFactory, nameof(addressFactory));

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var youngestAllowed = today.AddYears(-16);
        var oldestAllowed = today.AddYears(-120);

        if (dateOfBirth == default)
            return Result<Employee>.Failure("Ongeldige geboortedatum.");

        if (dateOfBirth < oldestAllowed || dateOfBirth > today)
            return Result<Employee>.Failure("Geboortedatum moet binnen de laatste 120 jaar liggen.");

        if (dateOfBirth > youngestAllowed)
            return Result<Employee>.Failure("Werknemer moet minstens 16 jaar oud zijn.");

        var employee = new Employee
        {
            FirstName = FirstName.Trim(),
            LastName = lastName.Trim(),
            DateOfBirth = dateOfBirth,
            Pin = EmployeeAuthentication.Create(pinHash)
        };

        var addressResults = addressFactory(employee).ToList();
        
        if (addressResults.Count == 0)
            return Result<Employee>.Failure("Een werknemer moet minstens één adres hebben.");
        
    foreach (var result in addressResults)
        {
            if (!result.IsSuccess)
                return Result<Employee>.Failure(result.Error);
                
            employee.EmployeeAddresses.Add(result.Value);
        }

        return Result<Employee>.Success(employee);
    }
  

}
