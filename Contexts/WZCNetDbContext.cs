using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WZCNet.Entities;

namespace WZCNet.Contexts;

public class WZCNetDbContext(DbContextOptions<WZCNetDbContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<EmployeeAddress> EmployeeAddresses { get; set; }
    public DbSet<ContactType> ContactTypes { get; set; }
    public DbSet<EmployeeContact> EmployeeContacts { get; set; }
    public DbSet<EmploymentHistory> EmploymentHistories { get; set; }
    public DbSet<EmployeeComment> EmployeeComments { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<EmployeePermission> EmployeePermissions { get; set; }
    public DbSet<JobTitle> JobTitles { get; set; }
    public DbSet<EmploymentHistoryJobTitle> EmploymentHistoryJobTitles { get; set; }
    public DbSet<JobTitlePermissions> JobTitlePermissions { get; set; }
    public DbSet<AppUser> AppUsers {get;set;}
    public DbSet<EmployeeUser> EmployeeUsers {get;set;}
    public DbSet<EmployeeAuthentication> EmployeeAuthentifications {get;set;}

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


        // Employee 
        modelBuilder.Entity<Employee>().HasMany(e => e.EmployeeAddresses).WithOne(ea => ea.Employee).HasForeignKey(ea => ea.EmployeeId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Employee>().HasMany(e => e.EmployeeContacts).WithOne(ct => ct.Employee).HasForeignKey(ct => ct.EmployeeId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Employee>().HasIndex(e => new { e.FirstName, e.LastName, e.DateOfBirth }).IsUnique().HasFilter("\"DeletedAt\" IS NULL");
        modelBuilder.Entity<Employee>().HasOne(e=>e.Pin).WithOne(p=>p.Employee).HasForeignKey<EmployeeAuthentication>(eaf=>eaf.EmployeeId).OnDelete(DeleteBehavior.Cascade).IsRequired(false);

        // contactTypes
        modelBuilder.Entity<ContactType>().HasIndex(ct => ct.TypeOfContact).IsUnique().HasFilter("\"DeletedAt\" IS NULL");
        modelBuilder.Entity<EmployeeContact>().HasOne(ec => ec.ContactType).WithMany().HasForeignKey(ec => ec.ContactTypeId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<ContactType>().HasData([
            new(){
                Id = 1,
                TypeOfContact = "Email",
                CreatedAt = DateTime.Parse("2026-07-01").ToUniversalTime()
            },
            new(){
                Id = 2,
                TypeOfContact = "GSM",
                CreatedAt = DateTime.Parse("2026-07-01").ToUniversalTime()
            }
        ]);

        //EmploymentHistorie
        modelBuilder.Entity<EmploymentHistory>().HasOne(eh => eh.Employee).WithMany(e => e.EmploymentHistories).HasForeignKey(eh => eh.EmployeeId).OnDelete(DeleteBehavior.Cascade);

        //EmployeeComment
        modelBuilder.Entity<EmployeeComment>().HasOne(ec => ec.Author).WithMany(e => e.CommentsAuthored).HasForeignKey(ec => ec.CreatedByEmployeeId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<EmployeeComment>().HasOne(ec => ec.Recipient).WithMany(e => e.CommentsRecieved).HasForeignKey(ec => ec.CreatedForEmployeeId).OnDelete(DeleteBehavior.Restrict);

        //Permission
        modelBuilder.Entity<Permission>().HasIndex(p => p.PermissionString).IsUnique().HasFilter("\"DeletedAt\" IS NULL");

        //EmployeePermission
        modelBuilder.Entity<EmployeePermission>().HasOne(ep => ep.Employee).WithMany(e => e.PersonalPermissions).HasForeignKey(ep => ep.EmployeeId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<EmployeePermission>().HasOne(ep => ep.Permission).WithMany().HasForeignKey(ep => ep.PermissionId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<EmployeePermission>().HasIndex(ep => new { ep.EmployeeId, ep.PermissionId });

        //jobtitle
        modelBuilder.Entity<JobTitle>().HasIndex(jt => jt.JobTitleString).IsUnique().HasFilter("\"DeletedAt\" IS NULL");
        modelBuilder.Entity<JobTitlePermissions>().HasOne(jtpm => jtpm.Permission).WithMany().HasForeignKey(jtpm => jtpm.PermissionId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<JobTitlePermissions>().HasOne(jtpm => jtpm.JobTitle).WithMany(jt => jt.JobTitlePermissions).HasForeignKey(jtpm => jtpm.JobTitleId).OnDelete(DeleteBehavior.Restrict);


        //employeeJobtitleHistorie
        modelBuilder.Entity<EmploymentHistoryJobTitle>().HasOne(ehjt => ehjt.EmploymentHistory).WithMany(eh => eh.EmploymentHistoryJobTitles).HasForeignKey(ehjt => ehjt.EmploymentHistoryId).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<EmploymentHistoryJobTitle>().HasOne(ehjt => ehjt.JobTitle).WithMany().HasForeignKey(ehjt => ehjt.JobTitleId).OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AppUser>().HasIndex(au=>au.UserName).IsUnique().HasFilter("\"DeletedAt\" IS NULL");
        // AppUser <-> EmployeeHistories many-to-many via EmployeeUser
        modelBuilder.Entity<EmployeeUser>().HasOne(eu => eu.EmploymentHistory).WithMany(eh=>eh.EmployeeUsers).HasForeignKey(eu => eu.EmploymentHistoryId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<EmployeeUser>().HasOne(eu => eu.AppUser).WithMany(au => au.EmployeeUsers).HasForeignKey(eu => eu.AppUserId).OnDelete(DeleteBehavior.Restrict);
        //modelBuilder.Entity<EmployeeUser>().HasIndex(eu => new { eu.EmployeeId, eu.AppUserId }).IsUnique().HasFilter("\"DeletedAt\" IS NULL"); // a history can be used on multiple accounts

    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
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
