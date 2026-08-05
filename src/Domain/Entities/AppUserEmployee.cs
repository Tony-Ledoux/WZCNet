using WZCNet.src.Domain.Entities.EmployeeAggregate;
using WZCNet.src.Domain.ValueObjects;


namespace WZCNet.src.Domain.Entities;

public class AppUserEmployee:BaseEntity
{
    public int AppUserId {get;private set;}
    public int EmployeeRawId {get;private set;}
    public EmployeeId EmployeeId => new(EmployeeRawId);
    public AppUser AppUser {get; private set;} = null!;
    public Employee Employee {get;private set;} = null!;

    private AppUserEmployee(){}

    internal static AppUserEmployee Create(EmployeeId employeeId)
    {
        return new AppUserEmployee
        {
            EmployeeRawId = employeeId.Value
        };
    }
}