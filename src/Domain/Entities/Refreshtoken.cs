using WZCNet.src.Domain.ValueObjects;
using System.Security.Cryptography;

namespace WZCNet.src.Domain.Entities;

public class Refreshtoken: BaseEntity
{
    public string RefreshToken {get; private set;}
    public DateTime ValidUntil {get;private set;}
    public SessionInfo Device {get; private set;}
  
    public int AppUserId {get;private set;}
    public AppUser AppUser {get;private set;}

    public static Refreshtoken GetRefreshtoken(int userId, SessionInfo info)
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return new Refreshtoken
        {
            RefreshToken = Convert.ToBase64String(randomNumber),
            ValidUntil = DateTime.UtcNow.AddDays(30),
            AppUserId = userId,
            Device = info
        };
    }
    public bool IsValid() => DeletedAt == null && DateTime.UtcNow <= ValidUntil; 
}
