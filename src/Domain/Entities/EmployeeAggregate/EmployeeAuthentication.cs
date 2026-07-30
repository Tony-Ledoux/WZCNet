using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace WZCNet.src.Domain.Entities.EmployeeAggregate;

public class EmployeeAuthentication
{
    public string PinHash {get;private set;}
    public DateTime? PinChangedAt {get;private set;}
    public DateTime? PinLastUsedAt {get;private set;}

    private EmployeeAuthentication(){}

    public static EmployeeAuthentication Create(string pin)
    {
        return new EmployeeAuthentication
        {
            PinHash = GetPinHasher().HashPassword(null,pin),
            PinChangedAt = null
        };
    }

    private static PasswordHasher<EmployeeAuthentication> GetPinHasher()
    {
        var PinHasher = new PasswordHasher<EmployeeAuthentication>();
        return PinHasher;
    }

    public void ChangePin(string newPin)
    {
        PinHash = GetPinHasher().HashPassword(null,newPin);
        PinChangedAt = DateTime.UtcNow;
    }

    public bool ValidatePin(string pinToChek)
    {
        bool isValid = GetPinHasher().VerifyHashedPassword(null, PinHash ,pinToChek) == PasswordVerificationResult.Success;
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
