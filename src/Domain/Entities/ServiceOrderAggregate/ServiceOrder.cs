using System;
using WZCNet.src.Domain.Entities.EmployeeAggregate;
using WZCNet.src.Domain.Interfaces;

namespace WZCNet.src.Domain.Entities.ServiceOrderAggregate;

public class ServiceOrder:BaseEntity,  IAggregateRoot
{
    public int? CreateByEmployeeId {get;set;}
    public Employee? CreatedByEmployee {get;set;}
    public int RoomId {get;set;}
    public Room Room {get;set;}
    public string Problem {get;set;}
    public int StatusId {get;set;}
    public ServiceOrderStatus Status {get;set;}
    public ICollection<ServiceOrderComment> Comments {get;set;}=[];
}
