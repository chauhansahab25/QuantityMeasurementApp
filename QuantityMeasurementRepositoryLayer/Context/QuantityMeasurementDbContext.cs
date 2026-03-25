using Microsoft.EntityFrameworkCore;
using QuantityMeasurementModelLayer.Entities;

namespace QuantityMeasurementRepositoryLayer.Context;

public class QuantityMeasurementDbContext : DbContext
{
    public QuantityMeasurementDbContext(DbContextOptions<QuantityMeasurementDbContext> options) : base(options)
    {
    }

    public DbSet<QuantityMeasurementEntity> QuantityMeasurements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<QuantityMeasurementEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.FirstValue).HasPrecision(18, 2);
            entity.Property(e => e.SecondValue).HasPrecision(18, 2);
            entity.Property(e => e.Result).HasPrecision(18, 2);
            
            entity.Property(e => e.FirstUnit).HasMaxLength(50).IsRequired();
            entity.Property(e => e.SecondUnit).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Operation).HasMaxLength(20).IsRequired();
            entity.Property(e => e.MeasurementType).HasMaxLength(20).IsRequired();
            entity.Property(e => e.ResultString).HasMaxLength(500);
            entity.Property(e => e.ErrorMessage).HasMaxLength(500);
            
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.IsError).HasDefaultValue(false);
            
            // Create indexes for better performance
            entity.HasIndex(e => e.Operation).HasDatabaseName("IX_QuantityMeasurements_Operation");
            entity.HasIndex(e => e.MeasurementType).HasDatabaseName("IX_QuantityMeasurements_MeasurementType");
            entity.HasIndex(e => e.CreatedAt).HasDatabaseName("IX_QuantityMeasurements_CreatedAt");
            entity.HasIndex(e => e.IsError).HasDatabaseName("IX_QuantityMeasurements_IsError");
        });
    }
}
