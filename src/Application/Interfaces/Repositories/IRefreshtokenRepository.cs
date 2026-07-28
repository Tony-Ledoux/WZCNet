using System;
using WZCNet.src.Domain.Entities;

namespace WZCNet.src.Application.Interfaces.Repositories;

public interface IRefreshtokenRepository
{
    Task<Refreshtoken?> GetRefreshtokenByTokenStringAsync(string token);
}
