using System;

namespace WZCNet.src.Domain.Entities;

public class Room: BaseEntity
{
    public string RoomName {get;set;}
    public int? DepartmentId {get;set;}
    public Department? Department {get;set;}
    public int FloorId {get;set;}
    public Floor Floor {get;set;}
    public bool CanHaveResident {get;set;}
}
