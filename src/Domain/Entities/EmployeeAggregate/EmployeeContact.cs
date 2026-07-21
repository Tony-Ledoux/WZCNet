using System.ComponentModel.DataAnnotations.Schema;

namespace WZCNet.src.Domain.Entities.EmployeeAggregate;

public class EmployeeContact: BaseEntity
{
    public int EmployeeId {get;set;}
    public int ContactTypeId {get;set;}
    public string ContactDetails {get;set;}


    public Employee Employee {get;set;}

    public ContactType ContactType {get;set;}
}
