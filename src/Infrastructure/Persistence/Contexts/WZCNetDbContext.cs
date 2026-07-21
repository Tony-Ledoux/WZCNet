
using Microsoft.EntityFrameworkCore;
using WZCNet.src.Domain.Interfaces;
using WZCNet.src.Domain.Entities;
using WZCNet.src.Domain.Entities.EmployeeAggregate;
using WZCNet.src.Domain.Entities.ServiceOrderAggregate;


namespace WZCNet.src.Infrastructure.Persistence.Contexts;

public class WZCNetDbContext(DbContextOptions<WZCNetDbContext> options) : DbContext(options)
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<EmployeeAddress> EmployeeAddresses {get;set;}
    public DbSet<ContactType> ContactTypes { get; set; }
    public DbSet<EmployeeContact> EmployeeContacts { get; set; }
    public DbSet<EmployeeEmploymentHistory> EmploymentHistories { get; set; }
    public DbSet<EmployeeEmploymentHistoryJobTitleAssignment> EmployeeEmploymentHistoryJobTitleAssignments {get;set;}
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<JobTitle> JobTitles { get; set; }
    public DbSet<EmployeeComment> EmployeeComments {get;set;}
    public DbSet<AppUser> AppUsers {get;set;}
    public DbSet<Floor> Floors {get;set;}
    public DbSet<Department> Departments {get;set;}
    public DbSet<Room> Rooms {get;set;}


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Employee 
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.OwnsOne(e => e.Pin, pinBuilder =>
            {
                pinBuilder.Property(p=>p.PinHash).IsRequired();
                pinBuilder.Property(p=>p.PinChangedAt).IsRequired(false);
                pinBuilder.Property(p=>p.PinLastUsedAt).IsRequired(false);
            });
            entity.OwnsMany(e=>e.EmployeeContacts, contactBuilder =>
            {
                contactBuilder.HasOne(ec=>ec.ContactType).WithMany().HasForeignKey(ec=>ec.ContactTypeId).OnDelete(DeleteBehavior.Restrict);
            });
            entity.HasMany(e=>e.EmploymentHistories).WithOne(eh=>eh.Employee).HasForeignKey(eh=>eh.EmployeeId).OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(e => new { e.FirstName,e.LastName, e.DateOfBirth }).IsUnique().HasFilter("\"DeletedAt\" IS NULL");
        });
        modelBuilder.Entity<EmployeeAddress>(entity =>
        {
            entity.HasOne(e=>e.Employee).WithMany(em=>em.Addresses).HasForeignKey(e=>e.EmployeeId).OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<ContactType>(entity =>
        {
            entity.HasIndex(ct=>ct.TypeOfContact).IsUnique().HasFilter("\"DeletedAt\" IS NULL");
        });
        //employmenthistory
        modelBuilder.Entity<EmployeeEmploymentHistory>(entity =>
        {
            entity.HasOne(eh=>eh.Employee).WithMany(e=>e.EmploymentHistories).HasForeignKey(eh=>eh.EmployeeId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(eh=>eh.JobTitleAssignments).WithOne(jta=>jta.EmploymentHistory).HasForeignKey(jta=>jta.EmploymentHistoryId).OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<EmployeeEmploymentHistoryJobTitleAssignment>(entity =>
        {
            entity.HasOne(ehjta=>ehjta.EmploymentHistory).WithMany(eh=>eh.JobTitleAssignments).HasForeignKey(ehjta=>ehjta.EmploymentHistoryId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(ehjta=>ehjta.JobTitle).WithMany().HasForeignKey(ehjta=>ehjta.JobTitleId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(ehjta=>ehjta.Department).WithMany().HasForeignKey(ehjta=>ehjta.PrincipalDepartmentId).OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<JobTitle>(entity =>
        {
           entity.HasIndex(jt=>jt.JobTitleString).IsUnique().HasFilter("\"DeletedAt\" IS NULL"); 
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasIndex(p=>p.PermissionString).IsUnique().HasFilter("\"DeletedAt\" IS NULL"); 
        });

        modelBuilder.Entity<EmployeePermission>(entity =>
        {
            entity.HasOne(ep=>ep.Employee).WithMany(e=>e.PersonalPermissions).HasForeignKey(ep=>ep.EmployeeId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(ep=>ep.Permission).WithMany().HasForeignKey(ep=>ep.PermissionId).OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(ep=> new{ep.EmployeeId,ep.PermissionId});
        });

        modelBuilder.Entity<JobTitlePermissions>(entity =>
        {
            entity.HasOne(jtpm=>jtpm.Permission).WithMany().HasForeignKey(jtpm=>jtpm.PermissionId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(jtpm=>jtpm.JobTitle).WithMany(jt=>jt.JobTitlePermissions).HasForeignKey(jtpm=>jtpm.JobTitleId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<EmployeeComment>(entity => 
        {
            entity.HasOne(ec=>ec.Author).WithMany(e=>e.CommentsAuthored).HasForeignKey(ec=>ec.CreatedByEmployeeId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(ec=>ec.Recipient).WithMany(e=>e.CommentsRecieved).HasForeignKey(ec=>ec.CreatedForEmployeeId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(ec=>ec.EmploymentHistory).WithMany().HasForeignKey(ec=>ec.CreatedDuringEmploymentId).OnDelete(DeleteBehavior.Restrict);
            entity.Property(ec=>ec.AuthorJobTitleSnapshot).IsRequired();
            entity.Property(ec=>ec.RecipientJobTitleSnapshot).IsRequired();
        });
        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasMany(a => a.Employees).WithMany(e => e.AppUsers).UsingEntity<AppUserEmployee>(je =>
            {
                je.HasOne(j=>j.AppUser).WithMany().HasForeignKey(j=>j.AppUsersId).OnDelete(DeleteBehavior.Cascade);
                je.HasOne(j=>j.Employee).WithMany().HasForeignKey(j=>j.EmployeeId).OnDelete(DeleteBehavior.Cascade);
            });
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasOne(r=>r.Floor).WithMany().HasForeignKey(r=>r.FloorId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(r=>r.Department).WithMany().HasForeignKey(r=>r.DepartmentId).OnDelete(DeleteBehavior.Restrict);
        });

        // ServiceOrders
        modelBuilder.Entity<ServiceOrderStatus>(entity =>
        {
            entity.HasIndex(sos=>sos.Status).IsUnique(true).HasFilter("\"DeletedAt\" IS NULL");
        });
        modelBuilder.Entity<ServiceOrder>(entity =>
        {
            entity.HasOne(so=>so.Room).WithMany().HasForeignKey(so=>so.RoomId).OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(so=>so.Status).WithMany().HasForeignKey(so=>so.StatusId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(so=>so.CreatedByEmployee).WithMany().HasForeignKey(so=>so.CreateByEmployeeId).OnDelete(DeleteBehavior.SetNull);
        });
        modelBuilder.Entity<ServiceOrderComment>(entity =>
        {
            entity.HasOne(c=>c.Author).WithMany().HasForeignKey(c=>c.AuthorId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(c=>c.ServiceOrder).WithMany(so=>so.Comments).HasForeignKey(c=>c.ServiceOrderId).OnDelete(DeleteBehavior.Cascade);
        });
       
       
         // Global query filter: automatically excludes soft-deleted records
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType) && !entityType.IsOwned())
            {
              var method = typeof(ModelBuilderExtensions).GetMethod(nameof(ModelBuilderExtensions.ApplySoftDeleteFilter))!.MakeGenericMethod(entityType.ClrType);
              method.Invoke(null,[modelBuilder]);
            }
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries().Where(e => e.Entity is ISoftDeletable))
        {
            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                ((ISoftDeletable)entry.Entity).DeletedAt = DateTime.UtcNow;
            }
        }
        foreach (var entry in ChangeTracker.Entries().Where(e => e.Entity is IAuditable))
        {
            var auditable = (IAuditable)entry.Entity;

            if (entry.State == EntityState.Added)
                auditable.CreatedAt = DateTime.UtcNow;
            else if (entry.State == EntityState.Modified)
                auditable.UpdatedAt = DateTime.UtcNow;
        }


        return base.SaveChangesAsync(cancellationToken);
    }


}
