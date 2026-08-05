using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WZCNet.src.Domain.Entities;

namespace WZCNet.src.Infrastructure.Persistence.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasMany(au=>au.Refreshtokens).WithOne(t=>t.AppUser).HasForeignKey(au=>au.AppUserId).OnDelete(DeleteBehavior.Cascade);
    }
}