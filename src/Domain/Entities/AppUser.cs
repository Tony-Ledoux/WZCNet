using WZCNet.src.Domain.Entities.EmployeeAggregate;

namespace WZCNet.src.Domain.Entities;
public class AppUser: BaseEntity
{
    public string UserName {get;set;}
    public string PasswordHash {get;set;}
    public bool IsPersonalAccount {get;set;}
    public bool IsActive {get;set;}
    public int NumberOfFailedLoginAttempts {get;set;}
    public DateTime? LastLogin {get;set;}
    public DateTime? PasswordLastChangedAt {get;set;}

    public ICollection<Employee> Employees {get;set;}=[];
    
}