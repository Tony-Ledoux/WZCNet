using System;
using System.ComponentModel.DataAnnotations.Schema;
using WZCNet.src.Domain.Entities;

namespace WZCNet.Entities;

public class EmployeeContact: BaseEntity
{
    public int EmployeeId {get;set;}
    public int ContactTypeId {get;set;}
    public string ContactDetails {get;set;}

    // navigational properties
    [ForeignKey(nameof(EmployeeId))]
    public Employee Employee {get;set;}

    [ForeignKey(nameof(ContactTypeId))]
    public ContactType ContactType {get;set;}
}
