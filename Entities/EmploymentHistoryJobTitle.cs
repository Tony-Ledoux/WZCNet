using System;

namespace WZCNet.Entities;

public class EmploymentHistoryJobTitle: BaseEntity
{
    public int EmploymentHistoryId {get;set;}
    public int JobTitleId {get;set;}
    public int? PrincipalDepartmentId {get;set;}
    public DateTime Start {get;set;}= DateTime.UtcNow;
    public DateTime? End {get;set;}

    // navigational properties
    public EmploymentHistory EmploymentHistory {get;set;}
    public JobTitle JobTitle {get;set;}
}
