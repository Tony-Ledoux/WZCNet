using System.Security.Cryptography;
using WZCNet.src.Domain.Common;
using WZCNet.src.Domain.Entities.EmployeeAggregate;

namespace WZCNet.src.Domain.Entities;
public class AppUser: BaseEntity
{
    public string UserName {get;set;}
    public string PasswordHash {get;set;}
    public bool IsPersonalAccount {get;set;}
    public bool IsActive {get;set;} = true;
    public int NumberOfFailedLoginAttempts {get;set;}
    public DateTime? LastLogin {get;set;}
    public DateTime? PasswordLastChangedAt {get;set;}
    public ICollection<Refreshtoken> Refreshtokens {get;set;}=[];
    public ICollection<Employee> Employees {get;set;}=[];

    private AppUser(){}
    public static Result<AppUser> Create(string userName,string passwordHash, bool IsPersonalAccount = false)
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

    
}