using WZCNet.src.Domain.Entities.EmployeeAggregate;
using WZCNet.src.Domain.Interfaces;

namespace WZCNet.src.Domain.Entities;

public class AppUserEmployee:BaseEntity
{
    public int EmployeeId {get;set;}
    public int AppUsersId {get;set;}
    public Employee Employee {get;set;}
    public AppUser AppUser {get;set;}
}