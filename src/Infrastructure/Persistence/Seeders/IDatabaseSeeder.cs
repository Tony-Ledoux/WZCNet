using System;

namespace WZCNet.src.Infrastructure.Persistence.Seeders;

public interface IDatabaseSeeder
{
    Task SeedAsync();
}
