using QuantityMeasurementModelLayer.Entities;

namespace QuantityMeasurementService.Interfaces
{
    public interface IQuantityMeasurementRepository
    {
        void Save(QuantityMeasurementEntity entity);
        Task SaveAsync(QuantityMeasurementEntity entity);
        List<QuantityMeasurementEntity> GetAll();
        Task<List<QuantityMeasurementEntity>> GetAllAsync();
        List<QuantityMeasurementEntity> GetByOperation(string operation);
        Task<List<QuantityMeasurementEntity>> GetByOperationAsync(string operation);
        List<QuantityMeasurementEntity> GetByMeasurementType(string measurementType);
        Task<List<QuantityMeasurementEntity>> GetByMeasurementTypeAsync(string measurementType);
        void DeleteAll();
        Task DeleteAllAsync();
        
        List<QuantityMeasurementEntity> GetByUserId(int userId);
        List<QuantityMeasurementEntity> GetByUserIdOperation(int userId, string operation);
        void DeleteByUserId(int userId);
    }
}
