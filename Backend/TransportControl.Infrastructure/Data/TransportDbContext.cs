using Microsoft.EntityFrameworkCore;
using TransportControl.Core.Entities;

namespace TransportControl.Infrastructure.Data;

/// <summary>
/// Contexto de base de datos principal para el sistema de control de transporte
/// </summary>
public class TransportDbContext : DbContext
{
    public TransportDbContext(DbContextOptions<TransportDbContext> options) : base(options)
    {
    }

    // DbSets para las entidades principales
    public DbSet<Trip> Trips { get; set; }
    public DbSet<Operator> Operators { get; set; }
    public DbSet<Place> Places { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de la entidad Trip
        ConfigureTripEntity(modelBuilder);
        
        // Configuración de la entidad Operator
        ConfigureOperatorEntity(modelBuilder);
        
        // Configuración de la entidad Place
        ConfigurePlaceEntity(modelBuilder);

        // Configurar actualización automática de ModifiedAt
        ConfigureAuditFields(modelBuilder);
    }

    private void ConfigureTripEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Trip>(entity =>
        {
            entity.HasKey(t => t.Id);
            
            entity.Property(t => t.ScheduledStartDateTime)
                .IsRequired();
                
            entity.Property(t => t.ScheduledEndDateTime)
                .IsRequired();
                
            entity.Property(t => t.Status)
                .IsRequired()
                .HasConversion<int>();

            entity.Property(t => t.EstimatedDistance)
                .HasPrecision(10, 2);
                
            entity.Property(t => t.ActualDistance)
                .HasPrecision(10, 2);

            entity.Property(t => t.Notes)
                .HasMaxLength(1000);
                
            entity.Property(t => t.VehicleId)
                .HasMaxLength(50);

            // Relaciones
            entity.HasOne(t => t.Origin)
                .WithMany(p => p.TripsAsOrigin)
                .HasForeignKey(t => t.OriginId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(t => t.Destination)
                .WithMany(p => p.TripsAsDestination)
                .HasForeignKey(t => t.DestinationId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(t => t.Operator)
                .WithMany(o => o.Trips)
                .HasForeignKey(t => t.OperatorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Índices
            entity.HasIndex(t => t.ScheduledStartDateTime);
            entity.HasIndex(t => t.Status);
            entity.HasIndex(t => new { t.OriginId, t.DestinationId });
        });
    }

    private void ConfigureOperatorEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Operator>(entity =>
        {
            entity.HasKey(o => o.Id);
            
            entity.Property(o => o.FirstName)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(o => o.LastName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(o => o.Email)
                .HasMaxLength(255);
                
            entity.Property(o => o.Phone)
                .HasMaxLength(20);
                
            entity.Property(o => o.EmployeeId)
                .HasMaxLength(50);
                
            entity.Property(o => o.LicenseNumber)
                .HasMaxLength(50);

            entity.Property(o => o.Status)
                .IsRequired()
                .HasConversion<int>();

            entity.Property(o => o.Address)
                .HasMaxLength(500);
                
            entity.Property(o => o.EmergencyContact)
                .HasMaxLength(200);
                
            entity.Property(o => o.EmergencyPhone)
                .HasMaxLength(20);

            // Índices
            entity.HasIndex(o => o.EmployeeId)
                .IsUnique()
                .HasFilter("[EmployeeId] IS NOT NULL");
                
            entity.HasIndex(o => o.LicenseNumber)
                .IsUnique()
                .HasFilter("[LicenseNumber] IS NOT NULL");
                
            entity.HasIndex(o => o.Email)
                .IsUnique()
                .HasFilter("[Email] IS NOT NULL");
        });
    }

    private void ConfigurePlaceEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Place>(entity =>
        {
            entity.HasKey(p => p.Id);
            
            entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);
                
            entity.Property(p => p.Code)
                .HasMaxLength(10);
                
            entity.Property(p => p.Description)
                .HasMaxLength(500);

            entity.Property(p => p.Address)
                .HasMaxLength(500);
                
            entity.Property(p => p.City)
                .HasMaxLength(100);
                
            entity.Property(p => p.State)
                .HasMaxLength(100);
                
            entity.Property(p => p.Country)
                .HasMaxLength(100);
                
            entity.Property(p => p.PostalCode)
                .HasMaxLength(20);

            entity.Property(p => p.Latitude)
                .HasPrecision(10, 7);
                
            entity.Property(p => p.Longitude)
                .HasPrecision(10, 7);

            entity.Property(p => p.Type)
                .IsRequired()
                .HasConversion<int>();
                
            entity.Property(p => p.Status)
                .IsRequired()
                .HasConversion<int>();

            entity.Property(p => p.ContactPerson)
                .HasMaxLength(200);
                
            entity.Property(p => p.ContactPhone)
                .HasMaxLength(20);
                
            entity.Property(p => p.ContactEmail)
                .HasMaxLength(255);
                
            entity.Property(p => p.SpecialInstructions)
                .HasMaxLength(1000);

            // Índices
            entity.HasIndex(p => p.Code)
                .IsUnique()
                .HasFilter("[Code] IS NOT NULL");
                
            entity.HasIndex(p => p.Name);
            entity.HasIndex(p => p.Type);
            entity.HasIndex(p => p.Status);
        });
    }

    private void ConfigureAuditFields(ModelBuilder modelBuilder)
    {
        // Configurar campos de auditoría para BaseEntity
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType, builder =>
                {
                    builder.Property(nameof(BaseEntity.CreatedAt))
                        .IsRequired()
                        .HasDefaultValueSql("GETUTCDATE()");
                        
                    builder.Property(nameof(BaseEntity.ModifiedAt))
                        .IsRequired()
                        .HasDefaultValueSql("GETUTCDATE()");

                    builder.Property(nameof(BaseEntity.CreatedBy))
                        .HasMaxLength(100);
                        
                    builder.Property(nameof(BaseEntity.ModifiedBy))
                        .HasMaxLength(100);
                });
            }
        }
    }

    public override int SaveChanges()
    {
        UpdateAuditFields();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditFields()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.ModifiedAt = DateTime.UtcNow;
            }
        }
    }
}