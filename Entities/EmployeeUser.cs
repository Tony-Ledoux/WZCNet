using System;

namespace WZCNet.Entities;

public class EmployeeUser: BaseEntity
{
    public int EmploymentHistoryId {get;set;}
    public int AppUserId {get;set;}
    public EmploymentHistory EmploymentHistory {get;set;}
    public AppUser AppUser {get;set;}
}
