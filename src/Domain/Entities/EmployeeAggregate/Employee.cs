using WZCNet.src.Domain.Common;
using WZCNet.src.Domain.ValueObjects;


namespace WZCNet.src.Domain.Entities.EmployeeAggregate;

public class Employee : BaseEntity
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public DateOnly DateOfBirth { get; set; }

    // navigation Property

    public EmployeeAuthentication? Pin { get; set; }

    public ICollection<EmployeeAddress> Addresses { get; private set; } = [];
    public ICollection<EmployeeContact> EmployeeContacts { get; private set; } = [];
    public ICollection<EmployeeEmploymentHistory> EmploymentHistories { get; private set; } = [];

    public ICollection<EmployeeComment> CommentsAuthored { get; set; } = [];
    public ICollection<EmployeeComment> CommentsRecieved { get; set; } = [];
    public ICollection<EmployeePermission> PersonalPermissions { get; set; } = [];
    public ICollection<AppUser> AppUsers {get;set;}=[];

    private Employee() { }
    public static Result<Employee> Create(string firstName, string lastName, DateOnly dateOfBirth, string pinHash, IEnumerable<EmployeeAddress> addresses)
    {
        ArgumentException.ThrowIfNullOrEmpty(firstName, nameof(firstName));
        ArgumentException.ThrowIfNullOrEmpty(lastName, nameof(lastName));
        ArgumentException.ThrowIfNullOrWhiteSpace(pinHash, nameof(pinHash));


        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var youngestAllowed = today.AddYears(-16);
        var oldestAllowed = today.AddYears(-120);

        if (dateOfBirth == default)
            return Result<Employee>.Failure("Ongeldige geboortedatum.");

        if (dateOfBirth < oldestAllowed || dateOfBirth > today)
            return Result<Employee>.Failure("Geboortedatum moet binnen de laatste 120 jaar liggen.");

        if (dateOfBirth > youngestAllowed)
            return Result<Employee>.Failure("Werknemer moet minstens 16 jaar oud zijn.");

        var nameResult = Name.Create(firstName, lastName);
        if (!nameResult.IsSuccess) return Result<Employee>.Failure(nameResult.Error);

        if (!addresses.Any()) return Result<Employee>.Failure("Een werknemer moet minstens één adres hebben.");

        var employee = new Employee
        {
          
            FirstName = nameResult.Value.First,
            LastName = nameResult.Value.Last,
            DateOfBirth = dateOfBirth,
            Pin = EmployeeAuthentication.Create(pinHash),
            Addresses = [.. addresses]
        };

        return Result<Employee>.Success(employee);
    }
    
    //TODO Add methods to manage contacts

    //TODO Add methods to manage employmentHistory and jobtitles


}
