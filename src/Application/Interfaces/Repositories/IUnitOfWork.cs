namespace WZCNet.src.Application.Interfaces.Repositories;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}