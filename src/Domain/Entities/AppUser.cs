using System.Security.Cryptography;
using Microsoft.AspNetCore.SignalR;
using WZCNet.src.Domain.Common;
using WZCNet.src.Domain.Entities.EmployeeAggregate;
using WZCNet.src.Domain.ValueObjects;

namespace WZCNet.src.Domain.Entities;

public class AppUser : BaseEntity
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
    }

    public void InvalidateExistingRefreshTokens(SessionInfo sessionInfo)
    {
        if (Refreshtokens == null || Refreshtokens.Count == 0) return;
        var existingTokens = Refreshtokens.Where(rt =>
            rt.Device?.DeviceInfo == sessionInfo.DeviceInfo &&
            rt.Device?.IpAddress == sessionInfo.IpAddress &&
            rt.DeletedAt == null).ToList();
        foreach (var tokenToInvalidate in existingTokens)
        {
            tokenToInvalidate.Invalidate();
        }
    }

    public Refreshtoken AddRefreshToken(SessionInfo session,int? employeeId)
    {
        var rf = Refreshtoken.CreateRefreshtoken(Id,session,employeeId);
        Refreshtokens.Add(rf);
        return rf;
    }

    public Refreshtoken? GetRefreshtokenByDeviceInfoAndEmployeeId(SessionInfo session, int? employeeId)
    {
        if (Refreshtokens == null || Refreshtokens.Count == 0) return null;
        return Refreshtokens.FirstOrDefault(t=>t.EmployeeId == employeeId && t.Device.IpAddress == session.IpAddress && t.Device.DeviceInfo == session.DeviceInfo);
    }

    public Employee? GetEmployeeById(int? id = null)
    {
        if(Employees.Count == 0) return null;
        if(IsPersonalAccount && Employees.Count == 1) return Employees.First();
        return Employees.FirstOrDefault(e=>e.Id == id);

    }

    public Refreshtoken? GetRefreshtokenByTokenString(string token)
    {
        return Refreshtokens.FirstOrDefault(t=>t.RefreshToken == token);
    }


}