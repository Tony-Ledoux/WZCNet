using System;

namespace WZCNet.Entities;

public class JobTitle: BaseEntity
{
    public string JobTitleString {get;set;}

    public ICollection<JobTitlePermissions> JobTitlePermissions {get;set;}= [];
}
