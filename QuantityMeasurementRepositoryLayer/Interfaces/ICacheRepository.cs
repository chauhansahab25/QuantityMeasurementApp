using QuantityMeasurementModelLayer.Entities;

namespace QuantityMeasurementRepositoryLayer.Interfaces;

public interface ICacheRepository : IQuantityMeasurementRepository
{
    // Check if there's pending data to upload
    bool HasPendingData();
    
    // Get all pending data for upload
    List<QuantityMeasurementEntity> GetPendingData();
    
    // Clear pending data after successful upload
    void ClearPendingData();
    
    // Clear JSON file
    void ClearJsonFile();
}