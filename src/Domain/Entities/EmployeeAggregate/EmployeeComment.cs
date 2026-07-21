namespace WZCNet.src.Domain.Entities.EmployeeAggregate;

public class EmployeeComment: BaseEntity
{
    public int CreatedByEmployeeId {get;set;}
    public string AuthorJobTitleSnapshot {get;set;}
    public int CreatedForEmployeeId {get;set;}
    public string RecipientJobTitleSnapshot {get;set;}
    public int CreatedDuringEmploymentId {get;set;}
    public bool IsPrivate {get;set;}
    public bool IsResolved {get;set;}

    //navigational properties
    
    public Employee Author {get;set;}
    public Employee Recipient {get;set;}
    public EmployeeEmploymentHistory EmploymentHistory {get;set;}
  
}
