namespace WZCNet.src.Domain.Entities.EmployeeAggregate;

public class EmployeeEmploymentHistory: BaseEntity
{
    public int EmployeeId {get;set;}
    public DateTime Start {get;set;}=DateTime.UtcNow;
    public DateTime? End {get;set;}
    public Employee Employee {get;set;}
    public ICollection<EmployeeEmploymentHistoryJobTitleAssignment> JobTitleAssignments {get;set;}=[];
    //public ICollection<EmployeeUser> EmployeeUsers {get;set;}=[];
}
