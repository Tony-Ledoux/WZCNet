using System;

namespace WZCNet.src.Application.DTOs;

public class TokenClaimsDTO
{
    public string UserName  {get;set;}
    public int UserAccountId {get;set;}
    public string? EmployeeName{get;set;}
    public int? EmployeeId {get;set;}
}
