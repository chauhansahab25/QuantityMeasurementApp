using QuantityMeasurementModelLayer.Entities;

namespace QuantityMeasurementRepositoryLayer.Interfaces;

public interface IQuantityMeasurementRepository
{
    // Async methods (preferred)
    Task SaveAsync(QuantityMeasurementEntity entity);
    Task<List<QuantityMeasurementEntity>> GetAllAsync();
    Task<(List<QuantityMeasurementEntity> Items, int TotalCount)> GetHistoryPagedAsync(int page, int pageSize, string? operation = null, string? measurementType = null);
    Task<List<QuantityMeasurementEntity>> GetByOperationAsync(string operation);
    Task<List<QuantityMeasurementEntity>> GetByMeasurementTypeAsync(string measurementType);
    Task<List<QuantityMeasurementEntity>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<List<QuantityMeasurementEntity>> GetErroredMeasurementsAsync();
    Task<int> GetTotalCountAsync();
    Task<int> GetCountByOperationAsync(string operation);
    Task DeleteAllAsync();
    Task<bool> OperationExistsAsync(double firstValue, string firstUnit, double secondValue, string secondUnit, string operation);
    Task<QuantityMeasurementEntity?> GetLastSavedOperationAsync();
    Task<bool> TestConnectionAsync();

    // Sync methods (for backward compatibility with existing business layer)
    void Save(QuantityMeasurementEntity entity);
    List<QuantityMeasurementEntity> GetAll();
    List<QuantityMeasurementEntity> GetByOperation(string operation);
    List<QuantityMeasurementEntity> GetByMeasurementType(string measurementType);
    int GetTotalCount();
    void DeleteAll();
    bool OperationExists(double firstValue, string firstUnit, double secondValue, string secondUnit, string operation);
    QuantityMeasurementEntity GetLastSavedOperation();
    bool TestConnection();
    void ResetIdentity();
}