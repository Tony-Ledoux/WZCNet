using WZCNet.src.Domain.Interfaces;

namespace WZCNet.src.Domain.Entities;

public class EmployeeAuthentication
{
    public int EmployeeId {get;set;}
    public string PinHash {get;set;}
    public DateTime? PinChangedAt {get;set;}
    public DateTime? PinLastUsedAt {get;set;}

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
