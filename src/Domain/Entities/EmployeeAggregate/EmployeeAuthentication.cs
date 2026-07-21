namespace WZCNet.src.Domain.Entities.EmployeeAggregate;

public class EmployeeAuthentication
{
    public string PinHash {get;private set;}
    public DateTime? PinChangedAt {get;private set;}
    public DateTime? PinLastUsedAt {get;private set;}

    private EmployeeAuthentication(){}

    public static EmployeeAuthentication Create(string pin_hash)
    {
        return new EmployeeAuthentication
        {
            PinHash = pin_hash,
            PinChangedAt = DateTime.UtcNow
        };
    }

    public void ChangePin(string newPinHash)
    {
        PinHash = newPinHash;
        PinChangedAt = DateTime.UtcNow;
    }

    public bool ValidatePin(string pinToChek)
    {
        bool isValid = PinHash == pinToChek;
        if (isValid)
        {
            PinLastUsedAt = DateTime.UtcNow;
        }
        return isValid;
    }

    public void MarkAsUsed()
    {
        PinLastUsedAt = DateTime.UtcNow;
    }

}
