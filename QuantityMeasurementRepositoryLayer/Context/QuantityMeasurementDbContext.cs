using Microsoft.EntityFrameworkCore;
using QuantityMeasurementModelLayer.Entities;

namespace QuantityMeasurementRepositoryLayer.Context;

public class QuantityMeasurementDbContext : DbContext
{
    public QuantityMeasurementDbContext(DbContextOptions<QuantityMeasurementDbContext> options) : base(options)
    {
    }

    public DbSet<QuantityMeasurementEntity> QuantityMeasurements { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserSession> UserSessions { get; set; }

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
            
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsError).HasDefaultValue(false);
            
            entity.HasIndex(e => e.Operation).HasDatabaseName("IX_QuantityMeasurements_Operation");
            entity.HasIndex(e => e.MeasurementType).HasDatabaseName("IX_QuantityMeasurements_MeasurementType");
            entity.HasIndex(e => e.CreatedAt).HasDatabaseName("IX_QuantityMeasurements_CreatedAt");
            entity.HasIndex(e => e.IsError).HasDatabaseName("IX_QuantityMeasurements_IsError");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Email).HasMaxLength(255).IsRequired();
            entity.Property(e => e.PasswordHash).HasMaxLength(100).IsRequired();
            entity.Property(e => e.FirstName).HasMaxLength(50).IsRequired();
            entity.Property(e => e.LastName).HasMaxLength(50).IsRequired();
            
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            
            entity.HasIndex(e => e.Email).IsUnique().HasDatabaseName("IX_Users_Email");
            entity.HasIndex(e => e.IsActive).HasDatabaseName("IX_Users_IsActive");
        });

        modelBuilder.Entity<UserSession>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.JwtTokenId).HasMaxLength(500).IsRequired();
            entity.Property(e => e.IpAddress).HasMaxLength(45).IsRequired();
            entity.Property(e => e.UserAgent).HasMaxLength(500);
            
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            
            entity.HasIndex(e => e.JwtTokenId).IsUnique().HasDatabaseName("IX_UserSessions_JwtTokenId");
            entity.HasIndex(e => e.UserId).HasDatabaseName("IX_UserSessions_UserId");
            entity.HasIndex(e => e.IsActive).HasDatabaseName("IX_UserSessions_IsActive");
            entity.HasIndex(e => e.ExpiresAt).HasDatabaseName("IX_UserSessions_ExpiresAt");
        });
    }
}
