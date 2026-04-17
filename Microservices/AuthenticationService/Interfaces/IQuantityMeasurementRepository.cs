using QuantityMeasurementModelLayer.Entities;

namespace AuthenticationService.Interfaces
{
    public interface IQuantityMeasurementRepository
    {
        void Save(QuantityMeasurementEntity entity);
        List<QuantityMeasurementEntity> GetAll();
        List<QuantityMeasurementEntity> GetByOperation(string operation);
        List<QuantityMeasurementEntity> GetByMeasurementType(string measurementType);
        void DeleteAll();
        List<QuantityMeasurementEntity> GetByUserId(int userId);
        List<QuantityMeasurementEntity> GetByUserIdOperation(int userId, string operation);
        List<QuantityMeasurementEntity> GetByUserIdMeasurementType(int userId, string measurementType);
        void DeleteByUserId(int userId);
    }
}
