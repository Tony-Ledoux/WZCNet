using WZCNet.src.Domain.Entities;

namespace WZCNet.src.Application.Interfaces.Repositories;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
    void Add<T>(T entity) where T:BaseEntity;
    void Remove<T>(T entity) where T:BaseEntity;
}