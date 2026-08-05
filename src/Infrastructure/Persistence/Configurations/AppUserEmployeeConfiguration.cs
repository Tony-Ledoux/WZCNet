using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WZCNet.src.Domain.Entities;
using WZCNet.src.Domain.ValueObjects;

namespace WZCNet.src.Infrastructure.Persistence.Configurations;

public class AppUserEmployeeConfiguration : IEntityTypeConfiguration<AppUserEmployee>
{
    public void Configure(EntityTypeBuilder<AppUserEmployee> builder)
    {
        builder.Ignore(x => x.EmployeeId);
        builder.HasKey(x=>x.Id);
        builder.HasIndex(x=>new {x.AppUserId,x.EmployeeRawId}).IsUnique();
        builder.HasOne(x=>x.AppUser).WithMany(x=>x.EmployeeLinks).HasForeignKey(x=>x.AppUserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(x=>x.Employee).WithMany(x=>x.AppUserLinks).HasForeignKey(x=>x.EmployeeRawId).OnDelete(DeleteBehavior.Cascade);
    }
}