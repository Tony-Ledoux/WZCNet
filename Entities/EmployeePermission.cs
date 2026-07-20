using System;
using WZCNet.src.Domain.Entities;

namespace WZCNet.Entities;

public class EmployeePermission:BaseEntity
{
    public int EmployeeId {get;set;}
    public int PermissionId {get;set;}
    public DateTime ValidFrom {get;set;}=DateTime.UtcNow;
    public DateTime? ValidUntil {get;set;}

    public Employee Employee {get;set;}
    public Permission Permission {get;set;}
}
