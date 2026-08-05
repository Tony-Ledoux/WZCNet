using WZCNet.src.Domain.Common;
using WZCNet.src.Domain.Entities.EmployeeAggregate;
using WZCNet.src.Domain.Interfaces;
using WZCNet.src.Domain.ValueObjects;


namespace WZCNet.src.Domain.Entities;

public class AppUser : BaseEntity, IAggregateRoot
{
    public string UserName { get; set; }
    public string PasswordHash { get; set; }
    public bool IsPersonalAccount { get; set; }
    public bool IsActive { get; set; } = true;
    public int NumberOfFailedLoginAttempts { get; set; }
    public DateTime? LastLogin { get; set; }
    public DateTime? PasswordLastChangedAt { get; set; }
    public ICollection<Refreshtoken> Refreshtokens { get; set; } = [];
    private readonly List<AppUserEmployee> _employeeLinks=[];
    public IReadOnlyCollection<AppUserEmployee> EmployeeLinks => _employeeLinks.AsReadOnly();

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
        if(NumberOfFailedLoginAttempts == 10)
        {
            IsActive = false;
        }
    }

    public void UnlockAccount()
    {
        IsActive = true;
        NumberOfFailedLoginAttempts = 0;
    }

    public Result<AppUser> AddEmployee(EmployeeId employeeId)
    {
        if(IsPersonalAccount && _employeeLinks.Count >= 1) return Result<AppUser>.Failure("A personal account can hold only one employee");
        if(_employeeLinks.Any(link=>link.EmployeeRawId == employeeId.Value)) return Result<AppUser>.Failure("This employee is already registered with this account");
        _employeeLinks.Add(AppUserEmployee.Create(employeeId));
        return Result<AppUser>.Success(this);
    }

    public Result<AppUser> RemoveEmployee(EmployeeId employeeId)
    {
        var link = _employeeLinks.FirstOrDefault(link => link.EmployeeRawId == employeeId.Value);
        if(link is null) return Result<AppUser>.Failure("This employee is not registered with this account");
        _employeeLinks.Remove(link);
        return Result<AppUser>.Success(this);
    }

    public EmployeeId? GetSingleEmployeeId()
{
    if (IsPersonalAccount && _employeeLinks.Count == 1)
        return _employeeLinks[0].EmployeeId;
    return null;
}


}