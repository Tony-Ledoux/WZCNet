
namespace WZCNet.src.Domain.Entities;

public class JobTitle: BaseEntity
{
    public string JobTitleString {get;private set;}

    private readonly List<JobTitlePermissions> _jobTitlePermissions = [];
    public virtual IReadOnlyCollection<JobTitlePermissions> JobTitlePermissions =>_jobTitlePermissions.AsReadOnly();

    //TODO Methods for Permissions
}
