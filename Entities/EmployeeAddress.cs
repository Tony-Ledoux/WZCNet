using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WZCNet.Entities;

public class EmployeeAddress: BaseEntity
{
    public int EmployeeId {get;set;}
    public string StreetName {get;set;}
    public string HouseNumber {get;set;}
    public string ZipCode {get;set;}
    public string Municipality {get;set;}
    public bool IsCurrent {get;set;}

    //navigational property
    [ForeignKey(nameof(EmployeeId))]
    public Employee Employee {get;set;}
}
