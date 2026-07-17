using System;

namespace WZCNet.Entities;

public class Employee: BaseEntity
{
    public string FirstName {get;set;}
    public string LastName {get;set;}
    public DateOnly DateOfBirth {get;set;}

    // navigation Property
    public ICollection<EmployeeAddress> EmployeeAddresses {get;set;} = [];
}
