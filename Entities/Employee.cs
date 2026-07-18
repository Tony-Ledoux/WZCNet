using System;

namespace WZCNet.Entities;

public class Employee: BaseEntity
{
    public string FirstName {get;set;}
    public string LastName {get;set;}
    public DateOnly DateOfBirth {get;set;}

    // navigation Property

    public EmployeeAuthentication? Pin {get;set;}

    public ICollection<EmployeeAddress> EmployeeAddresses {get;set;} = [];
    public ICollection<EmployeeContact> EmployeeContacts {get;set;}=[];
    public ICollection<EmploymentHistory> EmploymentHistories {get;set;}=[];

    public ICollection<EmployeeComment> CommentsAuthored {get;set;}=[];
    public ICollection<EmployeeComment> CommentsRecieved {get;set;}= [];
    public ICollection<EmployeePermission> PersonalPermissions {get;set;}= [];
  

}
