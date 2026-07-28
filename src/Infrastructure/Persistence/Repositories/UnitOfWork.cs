using WZCNet.src.Application.Interfaces.Repositories;
using WZCNet.src.Domain.Entities;
using WZCNet.src.Infrastructure.Persistence.Contexts;

namespace WZCNet.src.Infrastructure.Persistence.Repositories;

public class UnitOfWork(WZCNetDbContext context) : IUnitOfWork
{
    public void Add<T>(T entity) where T : BaseEntity => context.Set<T>().Add(entity);

    public void Remove<T>(T entity) where T : BaseEntity => context.Set<T>().Remove(entity);

    public async Task SaveChangesAsync() => await context.SaveChangesAsync();
    
}