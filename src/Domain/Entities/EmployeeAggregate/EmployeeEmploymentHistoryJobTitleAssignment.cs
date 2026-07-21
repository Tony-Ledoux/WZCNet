namespace WZCNet.src.Domain.Entities.EmployeeAggregate;

public class EmployeeEmploymentHistoryJobTitleAssignment: BaseEntity
{
    public int EmploymentHistoryId {get;set;}
    public EmployeeEmploymentHistory EmploymentHistory {get;set;}
    public int JobTitleId {get;set;}
    public JobTitle JobTitle {get;set;}
    public int? PrincipalDepartmentId {get;set;}
    public Department? Department {get;set;}
    public DateTime Start {get;set;}= DateTime.UtcNow;
    public DateTime? End {get;set;}

}
