using System.Security.Cryptography;
using Microsoft.AspNetCore.SignalR;
using Npgsql.Replication;
using WZCNet.src.Domain.Common;
using WZCNet.src.Domain.Entities.EmployeeAggregate;
using WZCNet.src.Domain.Interfaces;
using WZCNet.src.Domain.ValueObjects;

namespace WZCNet.src.Domain.Entities;

public class AppUser : BaseEntity, IAggregateRoot
{
    public string UserName { get; set; }
    public string PasswordHash { get; set; }
    public bool IsPersonalAccount { get; set; }
    public bool IsActive { get; set; } = true;
    public int NumberOfFailedLoginAttempts { get; set; }
    public DateTime? LastLogin { get; set; }
    public DateTime? PasswordLastChangedAt { get; set; }
    public ICollection<Refreshtoken> Refreshtokens { get; set; } = [];
    public ICollection<Employee> Employees { get; set; } = [];

    private AppUser() { }
    public static Result<AppUser> Create(string userName, string passwordHash, bool IsPersonalAccount = false)
    {
        // Validate required parameters
        if (string.IsNullOrWhiteSpace(userName)) return Result<AppUser>.Failure("username kan niet null of leeg zijn");

        if (string.IsNullOrWhiteSpace(passwordHash)) return Result<AppUser>.Failure("paswoordhash kan niet leeg of null zijn");
        var user = new AppUser
        {
            UserName = userName,
            PasswordHash = passwordHash,
            IsPersonalAccount = IsPersonalAccount
        };
        return Result<AppUser>.Success(user);
    }

    public void RegisterSuccessfulLogin()
    {
        LastLogin = DateTime.UtcNow;
        NumberOfFailedLoginAttempts = 0;
    }

    public void IncrementNumberOfFailedLoginAttempts()
    {
        NumberOfFailedLoginAttempts++;
        if(NumberOfFailedLoginAttempts == 10)
        {
            IsActive = false;
        }
    }

    public void UnlockAccount()
    {
        IsActive = true;
        NumberOfFailedLoginAttempts = 0;
    }

    public Result<AppUser> AddEmployee(Employee employee)
    {
        ArgumentNullException.ThrowIfNull(employee);

        if(IsPersonalAccount && Employees.Count == 1) return Result<AppUser>.Failure("A personal account can hold only one employee");
        if(Employees.Any(e=>e.Id == employee.Id)) return Result<AppUser>.Failure("This employee is already registered with this account");
        Employees.Add(employee);
        return Result<AppUser>.Success(this);
    }

    public Result<AppUser> RemoveEmployee(Employee employee)
    {
        ArgumentNullException.ThrowIfNull(employee);
        var linked_Employee = Employees.FirstOrDefault(e=>e.Id == employee.Id);
        if(linked_Employee == null) return Result<AppUser>.Failure("This employee is not registered with this account");
        Employees.Remove(linked_Employee);
        return Result<AppUser>.Success(this);
    }

    public int? GetSingleEmployeeId()
{
    if (IsPersonalAccount && Employees.Count == 1)
        return Employees.First().Id;
    return null;
}


}