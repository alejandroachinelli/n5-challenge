using Microsoft.EntityFrameworkCore;
using N5Challenge.Domain.Entities;

namespace N5Challenge.Infrastructure.Persistence.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<PermissionEntity> Permissions => Set<PermissionEntity>();
    public DbSet<PermissionTypeEntity> PermissionTypes => Set<PermissionTypeEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PermissionEntity>(entity =>
        {
            entity.ToTable("Permisos");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.EmployeeName).HasColumnName("NombreEmpleado").IsRequired();
            entity.Property(e => e.EmployeeLastName).HasColumnName("ApellidoEmpleado").IsRequired();
            entity.Property(e => e.PermissionDate).HasColumnName("FechaPermiso").IsRequired();
            entity.Property(e => e.PermissionTypeId).HasColumnName("TipoPermiso").IsRequired();

            entity.HasOne(e => e.PermissionType)
                  .WithMany(t => t.Permissions)
                  .HasForeignKey(e => e.PermissionTypeId);
        });

        modelBuilder.Entity<PermissionTypeEntity>(entity =>
        {
            entity.ToTable("TipoPermisos");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Description).HasColumnName("Descripcion").IsRequired();
        });

        base.OnModelCreating(modelBuilder);
    }
}
