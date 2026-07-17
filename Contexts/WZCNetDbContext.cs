using System;
using Microsoft.EntityFrameworkCore;

namespace WZCNet.Contexts;

public class WZCNetDbContext(DbContextOptions<WZCNetDbContext> options):DbContext(options)
{

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

}
