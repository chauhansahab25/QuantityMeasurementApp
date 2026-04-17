using QuantityMeasurementModelLayer.DTO;
using QuantityMeasurementModelLayer.Entities;

namespace QuantityMeasurementService.Interfaces;

public interface IQuantityMeasurementService
{
    bool Compare(QuantityDTO q1, QuantityDTO q2, int userId);

    QuantityDTO Convert(QuantityDTO input, string targetUnit, int userId);

    QuantityDTO Add(QuantityDTO q1, QuantityDTO q2, int userId);

    QuantityDTO Subtract(QuantityDTO q1, QuantityDTO q2, int userId);

    double Divide(QuantityDTO q1, QuantityDTO q2, int userId);
    
    List<QuantityMeasurementEntity> GetByUserId(int userId);
    List<QuantityMeasurementEntity> GetByUserIdOperation(int userId, string operation);
    void DeleteByUserId(int userId);
}