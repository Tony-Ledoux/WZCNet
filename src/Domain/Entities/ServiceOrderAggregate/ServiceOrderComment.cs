using WZCNet.src.Domain.Entities.EmployeeAggregate;

namespace WZCNet.src.Domain.Entities.ServiceOrderAggregate;
public class ServiceOrderComment:BaseEntity
{
    public string Comment {get;set;}
    public int ServiceOrderId {get;set;}
    public ServiceOrder ServiceOrder{get;set;}
    public int AuthorId {get;set;}
    public Employee Author {get;set;}
    
}