using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuantityMeasurementRepositoryLayer.Context;
using QuantityMeasurementRepositoryLayer.Interfaces;
using QuantityMeasurementModelLayer.Entities;

namespace QuantityMeasurementRepositoryLayer.Repositories;

public class QuantityMeasurementRepository : IQuantityMeasurementRepository
{
    private readonly QuantityMeasurementDbContext _context;
    private readonly ILogger<QuantityMeasurementRepository> _logger;

    public QuantityMeasurementRepository(QuantityMeasurementDbContext context, ILogger<QuantityMeasurementRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    // Async Methods
    public async Task SaveAsync(QuantityMeasurementEntity entity)
    {
        try
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            await _context.QuantityMeasurements.AddAsync(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Saved measurement: {entity.Operation} - {entity.Result}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving measurement");
            throw;
        }
    }

    public async Task<List<QuantityMeasurementEntity>> GetAllAsync()
    {
        try
        {
            return await _context.QuantityMeasurements
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all measurements");
            throw;
        }
    }

    public async Task<List<QuantityMeasurementEntity>> GetByOperationAsync(string operation)
    {
        try
        {
            return await _context.QuantityMeasurements
                .Where(m => m.Operation.Equals(operation, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting measurements by operation: {operation}");
            throw;
        }
    }

    public async Task<List<QuantityMeasurementEntity>> GetByMeasurementTypeAsync(string measurementType)
    {
        try
        {
            return await _context.QuantityMeasurements
                .Where(m => m.MeasurementType.Equals(measurementType, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting measurements by type: {measurementType}");
            throw;
        }
    }

    public async Task<List<QuantityMeasurementEntity>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        try
        {
            return await _context.QuantityMeasurements
                .Where(m => m.CreatedAt >= startDate && m.CreatedAt <= endDate)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting measurements by date range: {startDate} to {endDate}");
            throw;
        }
    }

    public async Task<List<QuantityMeasurementEntity>> GetErroredMeasurementsAsync()
    {
        try
        {
            return await _context.QuantityMeasurements
                .Where(m => m.IsError)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting errored measurements");
            throw;
        }
    }

    public async Task<int> GetTotalCountAsync()
    {
        try
        {
            return await _context.QuantityMeasurements.CountAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting total count");
            throw;
        }
    }

    public async Task<int> GetCountByOperationAsync(string operation)
    {
        try
        {
            return await _context.QuantityMeasurements
                .CountAsync(m => m.Operation.Equals(operation, StringComparison.OrdinalIgnoreCase));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting count by operation: {operation}");
            throw;
        }
    }

    public async Task DeleteAllAsync()
    {
        try
        {
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM QuantityMeasurements");
            await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('QuantityMeasurements', RESEED, 0)");
            _logger.LogInformation("All measurements deleted");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting all measurements");
            throw;
        }
    }

    public async Task<bool> OperationExistsAsync(double firstValue, string firstUnit, double secondValue, string secondUnit, string operation)
    {
        try
        {
            return await _context.QuantityMeasurements
                .AnyAsync(m => m.FirstValue == firstValue &&
                               m.FirstUnit == firstUnit &&
                               m.SecondValue == secondValue &&
                               m.SecondUnit == secondUnit &&
                               m.Operation == operation);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if operation exists");
            throw;
        }
    }

    public async Task<QuantityMeasurementEntity?> GetLastSavedOperationAsync()
    {
        try
        {
            return await _context.QuantityMeasurements
                .OrderByDescending(m => m.CreatedAt)
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting last saved operation");
            throw;
        }
    }

    public async Task<bool> TestConnectionAsync()
    {
        try
        {
            return await _context.Database.CanConnectAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Database connection test failed");
            return false;
        }
    }

    // Sync Methods (for backward compatibility)
    public void Save(QuantityMeasurementEntity entity)
    {
        SaveAsync(entity).GetAwaiter().GetResult();
    }

    public List<QuantityMeasurementEntity> GetAll()
    {
        return GetAllAsync().GetAwaiter().GetResult();
    }

    public List<QuantityMeasurementEntity> GetByOperation(string operation)
    {
        return GetByOperationAsync(operation).GetAwaiter().GetResult();
    }

    public List<QuantityMeasurementEntity> GetByMeasurementType(string measurementType)
    {
        return GetByMeasurementTypeAsync(measurementType).GetAwaiter().GetResult();
    }

    public int GetTotalCount()
    {
        return GetTotalCountAsync().GetAwaiter().GetResult();
    }

    public void DeleteAll()
    {
        DeleteAllAsync().GetAwaiter().GetResult();
    }

    public bool OperationExists(double firstValue, string firstUnit, double secondValue, string secondUnit, string operation)
    {
        return OperationExistsAsync(firstValue, firstUnit, secondValue, secondUnit, operation).GetAwaiter().GetResult();
    }

    public QuantityMeasurementEntity GetLastSavedOperation()
    {
        return GetLastSavedOperationAsync().GetAwaiter().GetResult() 
               ?? throw new InvalidOperationException("No operations found");
    }

    public bool TestConnection()
    {
        return TestConnectionAsync().GetAwaiter().GetResult();
    }

    public void ResetIdentity()
    {
        try
        {
            _context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('QuantityMeasurements', RESEED, 0)");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resetting identity");
            throw;
        }
    }
}
