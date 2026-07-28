using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WZCNet.src.Domain.Entities;

namespace WZCNet.src.Infrastructure.Persistence.Configurations;

public class RefreshtokenConfiguration : IEntityTypeConfiguration<Refreshtoken>
{
    public void Configure(EntityTypeBuilder<Refreshtoken> builder)
    {
       builder.OwnsOne(r=>r.Device, deviceBuilder =>
       {
           deviceBuilder.Property(d=>d.DeviceInfo).IsRequired(false);
           deviceBuilder.Property(d=>d.IpAddress).IsRequired(false);
       });
    }
}