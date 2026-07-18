using System;

namespace WZCNet.Entities;

public class JobTitlePermissions: BaseEntity
{
    public int JobTitleId {get;set;}
    public int PermissionId {get;set;}

    // navigational Properties
    public Permission Permission {get;set;}
    public JobTitle JobTitle {get;set;}
}
