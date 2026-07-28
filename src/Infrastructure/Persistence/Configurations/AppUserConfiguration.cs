using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WZCNet.src.Domain.Entities;

namespace WZCNet.src.Infrastructure.Persistence.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasMany(au=>au.Refreshtokens).WithOne(t=>t.AppUser).HasForeignKey(au=>au.AppUserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(au => au.Employees).WithMany(e => e.AppUsers).UsingEntity<AppUserEmployee>(je =>
        {
            je.HasOne(j=>j.Employee).WithMany().HasForeignKey(j=>j.EmployeeId).OnDelete(DeleteBehavior.Cascade);
            je.HasOne(j=>j.AppUser).WithMany().HasForeignKey(j=>j.AppUsersId).OnDelete(DeleteBehavior.Cascade);

        });
    }
}