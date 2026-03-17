// using QuantityMeasurementModelLayer.Entities;

// namespace QuantityMeasurementRepositoryLayer.Interfaces;

// public interface IQuantityMeasurementRepository
// {
//     void Save(QuantityMeasurementEntity entity);
//     List<QuantityMeasurementEntity> GetAll();
// }


using QuantityMeasurementModelLayer.Entities;

namespace QuantityMeasurementRepositoryLayer.Interfaces;

public interface IQuantityMeasurementRepository
{
    // Save measurement
    void Save(QuantityMeasurementEntity entity);

    // Get all measurements
    List<QuantityMeasurementEntity> GetAll();

    // Get measurements by operation (Add, Subtract, Compare etc.)
    List<QuantityMeasurementEntity> GetByOperation(string operation);

    // Get measurements by type (Length, Volume, Weight, Temperature)
    List<QuantityMeasurementEntity> GetByMeasurementType(string measurementType);

    // Total count of records
    int GetTotalCount();

    // Delete all measurements
    void DeleteAll();

    // Check if specific operation exists
    bool OperationExists(double firstValue, string firstUnit, double secondValue, string secondUnit, string operation);

    // Get the last saved operation
    QuantityMeasurementEntity GetLastSavedOperation();

    // Test database connectivity
    bool TestConnection();

    // Reset IDENTITY to proper value
    void ResetIdentity();
}