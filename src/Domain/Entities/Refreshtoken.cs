using WZCNet.src.Domain.ValueObjects;
using System.Security.Cryptography;
using WZCNet.src.Domain.Entities.EmployeeAggregate;

namespace WZCNet.src.Domain.Entities;

public class Refreshtoken: BaseEntity
{
    public string RefreshToken {get; private set;}
    public DateTime ValidUntil {get;private set;}
    public SessionInfo Device {get; private set;}
  
    public int AppUserId {get;private set;}
    public int? EmployeeId {get;private set;}
    public AppUser AppUser {get;private set;}
    public Employee? Employee {get;private set;}

    public static Refreshtoken CreateRefreshtoken(int userId, SessionInfo info, int? employeeId)
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return new Refreshtoken
        {
            RefreshToken = Convert.ToBase64String(randomNumber),
            ValidUntil = DateTime.UtcNow.AddDays(30),
            AppUserId = userId,
            Device = info,
            EmployeeId = employeeId
        };
    }
    public bool IsValid() => DeletedAt == null && DateTime.UtcNow < ValidUntil; 

    public void Invalidate()
    {
        DeletedAt = DateTime.UtcNow;
    }
}
