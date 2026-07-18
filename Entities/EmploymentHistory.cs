using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WZCNet.Entities;

public class EmploymentHistory: BaseEntity
{
    public int EmployeeId {get;set;}
    public DateTime Start {get;set;}=DateTime.UtcNow;
    public DateTime? End {get;set;}
    public Employee Employee {get;set;}
}
