using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WZCNet.Entities;

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


}
