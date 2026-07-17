using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WZCNet.Entities;

namespace WZCNet.Contexts;

public class WZCNetDbContext(DbContextOptions<WZCNetDbContext> options):DbContext(options)
{
    public DbSet<Employee> Employees {get;set;}
    public DbSet<EmployeeAddress> EmployeeAddresses {get;set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Global query filter: automatically excludes soft-deleted records
         foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(
                    GenerateIsNotNullFilter(entityType.ClrType)
                );
            }
        }


        // Employee --> EmployeeAddress
        modelBuilder.Entity<Employee>().HasMany(e=>e.EmployeeAddresses).WithOne(ea=>ea.Employee).HasForeignKey(ea=>ea.EmployeeId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Employee>().HasIndex(e=>new {e.FirstName, e.LastName,e.DateOfBirth}).IsUnique().HasFilter("\"DeletedAt\" IS NULL");

    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Deleted:
                    // Intercept hard delete → convert to soft delete
                    entry.State = EntityState.Modified;
                    entry.Entity.DeletedAt = DateTime.UtcNow;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
 // Helper function for the Query Filter
    private static LambdaExpression GenerateIsNotNullFilter(Type type)
    {
        var parameter = Expression.Parameter(type, "e");
        var property = Expression.Property(parameter, nameof(BaseEntity.DeletedAt));
        var compare = Expression.Equal(property, Expression.Constant(null));
        return Expression.Lambda(compare, parameter);
    }

}
