using System;
using WZCNet.src.Domain.Entities;

namespace WZCNet.Entities;

public class AppUser: BaseEntity
{
    public string UserName {get;set;}
    public string PasswordHash {get;set;}
    public bool IsInactive {get;set;}
    public int NumberOfFailedLogins {get;set;}
    public DateTime? LastLogin {get;set;}
    public DateTime? PasswordLastChangedAt {get;set;}

    //navigational properties
    public ICollection<EmployeeUser> EmployeeUsers {get;set;}=[];

}
