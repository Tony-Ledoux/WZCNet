using Microsoft.EntityFrameworkCore;
using WZCNet.src.Domain.Interfaces;

namespace WZCNet.src.Infrastructure.Persistence.Contexts;
public static class ModelBuilderExtensions
{
    public static void ApplySoftDeleteFilter<T>(ModelBuilder modelBuilder) where T : class, ISoftDeletable
    {
        modelBuilder.Entity<T>().HasQueryFilter(e=>e.DeletedAt == null);
    }
}