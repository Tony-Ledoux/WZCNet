using System;

namespace WZCNet.Entities;

public class EmployeeAuthentication:BaseEntity
{
    public int EmployeeId {get;set;}
    public string PinHash {get;set;}
    public DateTime? PinChangedAt {get;set;}
    public DateTime? LastUsedAt {get;set;}

    public Employee Employee {get;set;}

    private EmployeeAuthentication(){}

    public static EmployeeAuthentication Create(string pin_hash)
    {
        return new EmployeeAuthentication
        {
            PinHash = pin_hash,
            PinChangedAt = DateTime.UtcNow
        };
    }

}
